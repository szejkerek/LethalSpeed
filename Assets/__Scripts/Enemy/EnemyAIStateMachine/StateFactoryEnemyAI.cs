
public class StateFactoryEnemyAI
{
    StateMachineEnemyAI _context;

    public StateFactoryEnemyAI(StateMachineEnemyAI context)
    {
        _context = context;
    }

    public StateEnemyAI Idle()          => new IdleStateEnemyAI(_context, this);
    public StateEnemyAI SeekPlayer()    => new SeekingPlayerStateEnemyAI(_context, this);
    public StateEnemyAI Ragdoll()       => new RagdollStateEnemyAI(_context, this);
    public StateEnemyAI ShootPlayer()   => new ShootingPlayerStateEnemyAI(_context, this);
    public StateEnemyAI Crouch()        => new CrouchingStateEnemyAI(_context, this);
    public StateEnemyAI Reload()        => new ReloadingStateEnemyAI(_context, this);
    public StateEnemyAI Patrol()        => new PatrollingStateEnemyAI(_context, this);
    public StateEnemyAI WalkBackward()  => new WalkingBackwardsStateEnemyAI(_context, this);
    public StateEnemyAI Retrieve()      => new RetrievePositionStateEnemyAI(_context, this);
}
