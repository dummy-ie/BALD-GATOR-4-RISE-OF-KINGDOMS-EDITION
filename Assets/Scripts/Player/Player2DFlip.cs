using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DFlip : MonoBehaviour
{
    private float _x, _y, _z;
    private GameView _gameView;
    private SpriteRenderer _spriteRenderer;
    private PlayerController _playerController;


    [SerializeField]
    private bool Flip = false;

    void Start()
    {
        _x = transform.localScale.x;
        _y = transform.localScale.y;
        _z = transform.localScale.z;

        _gameView = ViewManager.Instance.GetComponentInChildren<GameView>();
        _playerController = transform.parent.gameObject.GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_gameView == null)
            Debug.LogError("No GameView in ViewManager!");

        if (_playerController == null)
            Debug.LogError("No Player Controller In Scene!");
    }
    void Update()
    {
        if (_playerController.IsMoving)
        {
            float xMovement = GameView.Input.x;

            if (Flip)
            {
                xMovement = -xMovement;
            }

            if (xMovement > 0)
                _spriteRenderer.flipX = false;
            else
                _spriteRenderer.flipX = true;

            // if (xMovement != 0)
            // {
            //     transform.localScale = new Vector3(_x * xMovement, _y, _z);
            // }
        }
    }
}