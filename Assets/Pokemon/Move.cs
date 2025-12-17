using UnityEngine;

public class Move
{
    public MoveBase Base { get; private set; }
    public int PP { get; set; }  // Current PP usage

    public Move(MoveBase moveBase)
    {
        Base = moveBase;
        PP = Base.PP;  // Changed from Base.Pp to Base.PP
    }
}
