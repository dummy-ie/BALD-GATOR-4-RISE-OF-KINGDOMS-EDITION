# BALD-GATOR-4-RISE-OF-KINGDOMS-EDITION
> complaints with code conventions or function ordering? ~~KYS~~ let us discuss in da discord

## Code Conventions
### just follow ms. candy's EXCEPT for the unnecessary "this." calls bc it just makes the code verbose
- _privateVars
- PublicVars
- FunctionsEitherPrivateOrPublic
    - ALWAYS specify access modifiers

## Code ordering (Top to bottom)
1. Serialized Variables (SerializeField and modifiers ABOVE variable)
2. Public Class Variables
3. Private Class Variables
4. User-defined Functions
5. Unity execution order
    - Refer to [text](https://docs.unity3d.com/Manual/ExecutionOrder.html):
    - ![alt](https://docs.unity3d.com/uploads/Main/monobehaviour_flowchart.svg)

