// GameEnums.cs
// Contiene las identificaciones específicas de las mecánicas de la partida.
// Se modifica o expande según las necesidades de los subordinados.

public enum WaveState  
{  
    Init,
    WaveInPrep,  
    WaveInProgress,  
    WaveCompleted
}

public enum EnemyID
{  
    ZomBunny = 0,  
    ZomBear = 1,  
    Hellephant = 2  
}  
  
public enum CollectibleID
{  
    Coin = 0,      
    Health = 1,  
    Ammo = 2,  
    RosaShield = 3,  
    ExtraLife = 4  
}