
/// <summary>
/// Efecto que realiza un hechizo cuando es jugado
/// </summary>
public class Effect
{
    /// <summary>
    /// Inicializa a efecto vacío
    /// </summary>
    public Effect()
    {
        Health = MaxHealth = Attack = 0;
        Taunt = Charge = Stealth = Windfury = false;
    }

    //Atributos generales
    public int Health;
    public int MaxHealth;

    //Atributos de minion
    public int Attack;
    public bool Taunt;
    public bool Charge;
    public bool Stealth;
    public bool Windfury;
}

