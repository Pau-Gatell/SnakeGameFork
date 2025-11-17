using UnityEngine;

[CreateAssetMenu(fileName = "GameStateGameplay", menuName = "GameStates/GSGameplay", order = 1)]
public class GSGameplay: GameState
{
    public override void OnEnter()
    {
        if (SlingshotController.instance != null)
        {
            SlingshotController.instance.isActive = true;
        }
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.Instance.ChangeGameState(StateType.PAUSE);
        }

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        if (enemies.Length < 1)
        {
            Debug.Log("We have won");
            GameStateManager.Instance.ChangeGameState(StateType.WIN);
        }
    }

    public override void OnExit()
    {
        SlingshotController.instance.isActive = false;
    }
}
