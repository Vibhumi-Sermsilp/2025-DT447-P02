using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WakeUp()
    {
        EnemyAI[] enemies = GetComponentsInChildren<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }

    public void ShutDown()
    {
        EnemyAI[] enemies = GetComponentsInChildren<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }
}
