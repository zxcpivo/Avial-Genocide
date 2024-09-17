using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour 
{
    public int health = 100;  
    public int damage = 10;   

  
    public Duck(int health, int damage)
    {
        this.health = health;
        this.damage = damage;
    }


    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("Duck is dead!");

    }
}

public class Duck1 : Duck
{
    public Duck1() : base(100, 10) { } 
}

public class Duck2 : Duck
{
    public Duck2() : base(150, 15) { } 
}

public class Duck3 : Duck
{
    public Duck3() : base(80, 5) { } 
}

public class Duck4 : Duck
{
    public Duck4() : base(120, 20) { } 
}


//sss