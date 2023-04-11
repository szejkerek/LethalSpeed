
public class StateFactoryEnemyAI
{
    StateMachineEnemyAI _context;

    public StateFactoryEnemyAI(StateMachineEnemyAI context)
    {
        _context = context;
    }

    public StateEnemyAI Idle()          => new IdleStateEnemyAI(_context, this);
    public StateEnemyAI SeekPlayer()    => new SeekPlayerStateEnemyAI(_context, this);
    public StateEnemyAI Ragdoll()       => new RagdollStateEnemyAI(_context, this);
}
