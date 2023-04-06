
public class EnemyAIStateFactory
{
    EnemyAIStateMachine _context;

    public EnemyAIStateFactory(EnemyAIStateMachine context)
    {
        context = _context;
    }

    public EnemyAIState Idle() { return new EnemyAIIdleState(_context, this); }
    public EnemyAIState SeekPlayer() { return new EnemyAISeekPlayerState(_context, this); }
    public EnemyAIState Ragdoll() { return new EnemyAIRagdollState(_context, this); }
}
