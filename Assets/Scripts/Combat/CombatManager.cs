using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static Entity;

public class CombatManager : Singleton<CombatManager>
{
    public enum CombatState
    {
        None,
        Start,
        PlayerTurn,
        EnemyTurn,
        Won, // unused!
        Lost // unused!
    }

    [SerializeField]
    public Transform NavigationTarget;
    public GameObject CurrentSelected;
    public CombatState State;

    [SerializeField]
    private Combatant _currentTurn;
    public Combatant CurrentTurn { get { return _currentTurn; } }
    
    [SerializeField]
    private GameObject _textPopupPrefab;
    public GameObject TextPopupPrefab { get { return _textPopupPrefab; } }

    private int _currentTurnIndex = 0;

    [SerializeField]
    private CinemachineVirtualCamera _overHeadCam;
    // private bool _isOverHeadCameraActive = false;

    [SerializeField]
    private List<Combatant> _combatants;
    public List<Combatant> Combatants { get { return _combatants; } }

    // debug
    public List<Entity> Entities;

    public bool StartFight = false;

    public IEnumerator StartCombat(List<Entity> entities)
    {
        Entities = entities;


        State = CombatState.Start;
        foreach (Entity entity in entities)
        {
            // roll for initiative
            InternalDice.Instance.Roll(out entity.Initiative, 20, entity.Class.GetModifier(entity.Class.Dexterity));

            // turn off physics for the players while battle is ongoing
            if (entity.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = true;
            }

            if (entity.gameObject.TryGetComponent(out FocusCameraOnTap focus))
                focus.enabled = true;

            if (entity.gameObject.TryGetComponent(out NavMeshAgent agent))
                agent.enabled = true;

            // entity.gameObject.FindComponentAndSetActive<Combatant>(true, out _);
            // entity.gameObject.FindComponentAndSetActive<FocusCameraOnTap>(true, out _);
            entity.gameObject.FindComponentAndSetActive<AnimatedHighlight>(true, out _);
            // entity.gameObject.FindComponentsAndSetActive<Projector>(true, out _);
            entity.gameObject.FindComponentsAndSetActive<LineRenderer>(true, out _);

            CinemachineVirtualCamera cam = entity.GetComponentInChildren<CinemachineVirtualCamera>(true);
            if (cam != null)
            {
                CinemachineOrbitalTransposer orbit = cam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
                if (orbit != null)
                    orbit.m_Heading.m_Bias = 0;
            }

            if (entity.TryGetComponent(out Combatant combatant))
            {
                combatant.enabled = true;
                _combatants.Add(combatant);
            }
            else
                Debug.LogError("Entity " + entity.name + " does not have a Combatant component!");
        }

        // wait a little bit before starting 
        // possibly insert animation
        yield return new WaitForSeconds(2f);
        // ViewManager.Instance.HideRecentView();
        AudioManager.Instance.StartBGM(EBGMIndex.BATTLE, 1);
        AudioManager.Instance.PauseBGM(0);
        ViewManager.Instance.GetView<GameView>().Hide();
        //ViewManager.Instance.GetView<DialogueView>().Hide();
        ViewManager.Instance.Show<CombatView>();
        ViewManager.Instance.GetView<CombatView>().ToggleHotbar();

        // Sort combatants by descending initiative
        _combatants = _combatants.OrderByDescending(e => e.Data.Initiative).ToList();
        // normalize the initiative rolls 
        for (int i = 0; i < _combatants.Count; i++)
        {
            // Debug.Log(_combatants[i].name + "'s Initiative roll: " + _combatants[i].Data.Initiative);
            _combatants[i].Data.Initiative = i;
        }

        if (_combatants.Count <= 1)
        {
            Debug.LogError("Too Few Combatants in Combat!");
            yield break;
        }

        StartCoroutine(StartNextTurn());
    }

    public void EndCombat()
    {
        foreach (Entity entity in Entities)
        {
            // re-enable whatever we turned off
            if (entity.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = false;
            }

            // if (entity.TryGetComponent(out PlayerController controller))
            // {
            //     controller.
            // }

            CinemachineVirtualCamera cam = entity.GetComponentInChildren<CinemachineVirtualCamera>(true);
            cam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_Bias = -180;

            if (entity.Health > 0)
                ICameraManipulator.SwitchCamera(cam);

            // disable whatever we turned on
            // entity.gameObject.FindComponentAndSetActive<Combatant>(false, out _);
            // entity.gameObject.FindComponentAndSetActive<FocusCameraOnTap>(false, out _);
            // if (entity.gameObject.TryGetComponent(out FocusCameraOnTap focus) && entity.Affiliation != AffiliationState.Ally)
            //     focus.enabled = false;

            // if (entity.gameObject.TryGetComponent(out Combatant combatant))
            //     combatant.enabled = false;

            // if (entity.gameObject.TryGetComponent(out NavMeshAgent agent))
            //     agent.enabled = false;

            entity.gameObject.FindComponentAndSetActive<AnimatedHighlight>(false, out _);
            entity.gameObject.FindComponentsAndSetActive<Projector>(false, out _);
            entity.gameObject.FindComponentsAndSetActive<LineRenderer>(false, out _);

        }

        NavigationTarget.gameObject.SetActive(false);
        _currentTurn = null;
        _combatants.Clear();
        AudioManager.Instance.PlayBGM(0);
        AudioManager.Instance.PauseBGM(1);
        ViewManager.Instance.GetView<CombatView>().ToggleHotbar();
        ViewManager.Instance.GetView<CombatView>().SetTargetData();
        State = CombatState.None;

        // ViewManager.Instance.HideRecentView();
        ViewManager.Instance.Show<GameView>();
    }

    public void SwitchCameraToOverhead()
    {
        if (_overHeadCam == null)
        {
            Debug.LogError("Overhead cam is null!");
            return;
        }


        // if the camera is active, press the button to recenter to the current turn.
        if (_overHeadCam.gameObject.activeSelf)
        {
            _overHeadCam.transform.position = _currentTurn.transform.position + (Vector3.up * 5f);
        }
        else // otherwise just activate it
            _overHeadCam.gameObject.SetActive(!_overHeadCam.gameObject.activeSelf);
    }

    public void AttackSelectedTarget()
    {
        CurrentSelected.TryGetComponent(out Combatant combatant);
        if (combatant == null)
        {
            Debug.LogError("Could not find a Combatant script from the selected target!");
            return;
        }

        if (_currentTurn.Data.ActionsLeft <= 0)
        {
            Debug.Log(_currentTurn.name + " trying to attack, but they don't have enough action points.");
            return;
        }

        if (_currentTurn.Data.Affiliation == combatant.Data.Affiliation)
        {
            Debug.Log(_currentTurn.name + " trying to attack " + CurrentSelected.name + ", but they have the same affiliation.");
            return;
        }

        if (_currentTurn.Data.Class.Attack.Range < Vector3.Distance(_currentTurn.transform.position, combatant.transform.position))
        {
            Debug.Log(_currentTurn.name + " trying to attack " + CurrentSelected.name + ", but the attack won't reach.");
            return;
        }

        // Scriptable Object Attack
        string text = combatant.TryHit(_currentTurn.Data.Class.Attack);
        Color color = Color.red;
        if (text == "Miss!")
            color = Color.white;
            
        if (TextPopupPrefab != null)
        {
            GameObject popup = Instantiate(TextPopupPrefab, combatant.transform.position, Quaternion.identity);
            if (popup.TryGetComponent(out TextPopup component))
            {
                component.Initialize(text, color);
                component.Activate = true;
            }
        }
        else
            Debug.LogError("Text Popup prefab is null!");


        RefreshUI();
        _currentTurn.DecrementAction();
    }

    public void HealSelectedTarget()
    {
        CurrentSelected.TryGetComponent(out Combatant combatant);
        if (combatant == null)
        {
            Debug.LogError("Could not find a Combatant script from the selected target!");
            return;
        }

        if (_currentTurn.Data.ActionsLeft <= 0)
        {
            Debug.Log(_currentTurn.name + " trying to heal, but they don't have enough action points.");
            return;
        }

        if (_currentTurn.Data.Affiliation != combatant.Data.Affiliation)
        {
            Debug.Log(_currentTurn.name + " trying to heal " + CurrentSelected.name + ", but they have different affiliations.");
            return;
        }

        if (_currentTurn.Data.Class.Heal.Range < Vector3.Distance(_currentTurn.transform.position, combatant.transform.position))
        {
            Debug.Log(_currentTurn.name + " trying to heal " + CurrentSelected.name + ", but the heal won't reach.");
            return;
        }

        // Scriptable Object Heal
        string text = combatant.HealHealth(_currentTurn.Data.Class.Heal);

        if (TextPopupPrefab != null)
        {
            GameObject popup = Instantiate(TextPopupPrefab, combatant.transform.position, Quaternion.identity);
            if (popup.TryGetComponent(out TextPopup component))
            {
                component.Initialize(text, Color.green);
                component.Activate = true;
            }
        }
        else
            Debug.LogError("Text Popup prefab is null!");

        RefreshUI();
        _currentTurn.DecrementAction();
    }

    private void RefreshUI()
    {
        if (_currentTurn != null)
            ViewManager.Instance.GetView<CombatView>().SetCurrentTurnData(_currentTurn);

        if (CurrentSelected != null && CurrentSelected.TryGetComponent(out Combatant combatant))
            ViewManager.Instance.GetView<CombatView>().SetTargetData(combatant);
    }

    public void EndCurrentTurn()
    {
        Debug.Log("Ending turn...");
        _currentTurn.EndTurn = true;
    }

    // private void InstantiateEnemy()
    // {
    //     // import Entity stats to enemy
    // }

    // private void InstantiateAlly()
    // {
    //     // import Entity stats to ally
    // }

    private void IncrementTurnIndex()
    {
        if (_currentTurnIndex + 1 < _combatants.Count)
            _currentTurnIndex++;
        else
            _currentTurnIndex = 0;
    }

    private IEnumerator StartNextTurn()
    {
        // StopAllCoroutines();
        _currentTurn = _combatants[_currentTurnIndex];

        // if (_currentTurn.Data.Health <= 0)
        // {
        //     Debug.Log("next turn is dead, going next");
        //     IncrementTurnIndex();
        //     StartCoroutine(StartNextTurn());
        //     yield break;
        // }

        // Change UI to show current combatant's turn
        ViewManager.Instance.GetView<CombatView>().SetCurrentTurnData(_currentTurn);

        // enable current turn's ranges
        _currentTurn.gameObject.FindComponentsAndSetActive<Projector>(true, out _);

        // Move Camera to current turn
        _currentTurn.OnTap(new TapEventArgs(Vector2.zero));
        _currentTurn.GetComponent<FocusCameraOnTap>().OnTap(new TapEventArgs(Vector2.zero)); // whatever! fuck you

        AffiliationState current = _combatants[_currentTurnIndex].Data.Affiliation;

        if (current == AffiliationState.Enemy)
            State = CombatState.EnemyTurn;
        else
            State = CombatState.PlayerTurn;

        // just make them do their turn and wait until they choose to end it
        yield return StartCoroutine(_currentTurn.StartTurn());
        _currentTurn.gameObject.FindComponentsAndSetActive<Projector>(false, out _);

        // set index to next turn
        IncrementTurnIndex();

        // check if won or lost 
        if (CheckWin())
        {
            Debug.Log("Win");
            StartCoroutine(DialogueManager.Instance.EndBattleState(true));
            EndCombat();
            
        }
        else if (CheckLoss())
        {
            Debug.Log("Loss");
            StartCoroutine(DialogueManager.Instance.EndBattleState(false));
            EndCombat();
            
            SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // else repeat
            StartCoroutine(StartNextTurn());
        }
    }

    private bool CheckWin() // linq my beloved
    {
        return _combatants.Where(c => c.Data.Affiliation == AffiliationState.Enemy).All(c => c.Data.Health <= 0);
    }

    private bool CheckLoss()
    {
        return _combatants.Where(c => c.Data.Affiliation == AffiliationState.Ally).All(c => c.Data.Health <= 0);
    }

    private void OnTap(object sender, TapEventArgs args)
    {
        //Debug.Log("Object hit by CombatManager OnTap(): " + args.HitObject.name);
        if (ViewManager.Instance.GetView<CombatView>() != null)
            ViewManager.Instance.GetView<CombatView>().SetTargetData(); // null to hide it

        if (args.HitObject != null && args.HitObject.CompareTag("Walkable"))
        {
            Ray ray = Camera.main.ScreenPointToRay(args.Position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                // holy mother of if statements i am not sorry for creating you lmao
                if (State != CombatState.None
                && CurrentSelected != null
                && CurrentSelected.TryGetComponent(out Combatant entity)
                && _currentTurn == entity
                && _currentTurn.Data.Affiliation == AffiliationState.Ally
                )
                {
                    NavigationTarget.gameObject.SetActive(true);
                    NavigationTarget.position = hit.point + Vector3.up * 0.1f;
                    entity.StartMove();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        State = CombatState.None;
        GestureManager.Instance.OnTap += OnTap;
        NavigationTarget.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFight)
        {
            StartFight = false;
            StartCoroutine(StartCombat(Entities));
        }
    }
}
