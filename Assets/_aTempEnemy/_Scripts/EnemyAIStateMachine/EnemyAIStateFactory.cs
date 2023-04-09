
public class EnemyAIStateFactory
{
    EnemyAIStateMachine _context;

    public EnemyAIStateFactory(EnemyAIStateMachine context)
    {
        _context = context;
    }

    public EnemyAIState Idle()          => new EnemyAIIdleState(_context, this);
    public EnemyAIState SeekPlayer()    => new EnemyAISeekPlayerState(_context, this);
    public EnemyAIState Ragdoll()       => new EnemyAIRagdollState(_context, this);
}
