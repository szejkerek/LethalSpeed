using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class DeveloperOptions : MonoBehaviour
{
    private const string _developerFolder = "Developer/";

//Keep all dev options here to be able to create builds
#if UNITY_EDITOR

    [MenuItem(_developerFolder + "Enemy/Kill all enemies")]
    public static void KillAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Die();
        }
    }

    [MenuItem(_developerFolder + "Enemy/State/Idle")]
    public static void IdleEnemyAI() { ChangeAllEnemiesState("Idle"); }


    [MenuItem(_developerFolder + "Enemy/State/Patrol")]
    public static void PatrolEnemyAI() { ChangeAllEnemiesState("Patrol"); }


    [MenuItem(_developerFolder + "Enemy/State/Crouch")]
    public static void CrouchEnemyAI() { ChangeAllEnemiesState("Crouch"); }


    [MenuItem(_developerFolder + "Enemy/State/Seek")]
    public static void SeekEnemyAI() { ChangeAllEnemiesState("Seek"); }


    [MenuItem(_developerFolder + "Enemy/State/Shoot")]
    public static void ShootEnemyAI() { ChangeAllEnemiesState("Shoot"); }


    [MenuItem(_developerFolder + "Enemy/State/WalkBackward")]
    public static void WalkBackwardEnemyAI() { ChangeAllEnemiesState("WalkBackward"); }


    [MenuItem(_developerFolder + "Enemy/State/Ragdoll")]
    public static void RagdollEnemyAI() { ChangeAllEnemiesState("Ragdoll"); }    
    
    [MenuItem(_developerFolder + "Enemy/State/Retrieve")]
    public static void RetrieveEnemyAI() { ChangeAllEnemiesState("Retrieve"); }


    static void ChangeAllEnemiesState(string stateName)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            StateMachineEnemyAI stateMachine = enemy.GetComponent<StateMachineEnemyAI>();
            switch (stateName)
            {
                case "Idle":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.Idle());
                    break;
                case "Patrol":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.Patrol());
                    break;
                case "Crouch":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.Crouch());
                    break;
                case "Seek":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.SeekPlayer());
                    break;
                case "Shoot":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.ShootPlayer());
                    break;
                case "WalkBackward":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.Flee());
                    break;
                case "Ragdoll":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.Ragdoll());
                    break;                
                case "Retrieve":
                    stateMachine.CurrentState.SwitchState(stateMachine.StatesFactory.Retrieve());
                    break;
            }
        }
    }


#endif
}

