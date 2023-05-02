
public class StateFactoryEnemyAI
{
    StateMachineEnemyAI _context;

    public StateFactoryEnemyAI(StateMachineEnemyAI context)
    {
        _context = context;
    }

    public StateEnemyAI Idle()          => new IdleStateEnemyAI(_context, this, "Idle");
    public StateEnemyAI SeekPlayer()    => new SeekingPlayerStateEnemyAI(_context, this, "SeekPlayer");
    public StateEnemyAI Ragdoll()       => new RagdollStateEnemyAI(_context, this, "Ragdoll");
    public StateEnemyAI ShootPlayer()   => new ShootingPlayerStateEnemyAI(_context, this, "ShootPlayer");
    public StateEnemyAI Crouch()        => new CrouchingStateEnemyAI(_context, this, "Crouch");
    public StateEnemyAI Reload()        => new ReloadingStateEnemyAI(_context, this, "Reload");
    public StateEnemyAI Patrol()        => new PatrollingStateEnemyAI(_context, this, "Patrol");
    public StateEnemyAI WalkBackward()  => new WalkingBackwardsStateEnemyAI(_context, this, "WalkBackward");
    public StateEnemyAI Retrieve()      => new RetrievePositionStateEnemyAI(_context, this, "Retrieve");
}
