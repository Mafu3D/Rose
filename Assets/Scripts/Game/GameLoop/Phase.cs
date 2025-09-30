// namespace Project.GameLoop
// {
//     public abstract class Phase
//     {
//         public enum Status { Running, Complete };
//         public readonly Phase Child;
//         protected GameManager GameManager;
//         protected StateMachine StateMachine;
//         public string Name;
//         public float TimeInPhase { get; private set; } = 0f;
//         public void UpdateTimeInPhase(float deltaTime) => TimeInPhase += deltaTime;

//         public Phase(string name, StateMachine stateMachine, GameManager gameManager)
//         {
//             this.Name = name;
//             this.GameManager = gameManager;
//             this.StateMachine = stateMachine;
//         }

//         public abstract Status OnEnter();
//         public abstract Status Update(float time);
//         public abstract void OnExit();
//     }

//     public class GamePhaseProcessor
//     {
//         Phase
//     }
// }