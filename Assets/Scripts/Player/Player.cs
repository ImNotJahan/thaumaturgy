using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    Bar healthBar;
    [SerializeField]
    Bar manaBar;

    [SerializeField]
    int MAX_MANA = 1000;
    [SerializeField]
    float mana;

    float timeSinceManaUsed = 0;
    [SerializeField]
    float minTimeBetweenManaUseAndRegeneration = 3f;
    [SerializeField]
    float manaRegenPerSecond = 100f;

    protected override void Start()
    {
        base.Start();
        mana = MAX_MANA;
    }

    public override void Hurt(int amount)
    {
        base.Hurt(amount);
        healthBar.SetFillPercent(health / (float)MAX_HEALTH);
    }

    protected override void Die()
    {

    }

    public void UseMana(int amount)
    {
        mana -= amount;
        manaBar.SetFillPercent(mana / MAX_MANA);
        timeSinceManaUsed = 0;
    }

    void Update()
    {
        timeSinceManaUsed += Time.deltaTime;

        if (timeSinceManaUsed >= minTimeBetweenManaUseAndRegeneration && mana < 1000)
        {
            mana += Time.deltaTime * manaRegenPerSecond;
            manaBar.SetFillPercent(mana / MAX_MANA);
        }
    }
}
