<p align="center"><h1>Lethal Speed</h1></p>
<p align="center">

  <a>
    <a href="https://unity.com/">
    <img alt="Made With Unity" src="https://img.shields.io/badge/made%20with-Unity-57b9d3.svg?logo=Unity">
    </a>
  <a>
  <img alt="License" src="https://img.shields.io/github/license/szejkerek/LethalSpeed?logo=github">
  </a>
  <a>
    <a href="https://github.com/szejkerek/LethalSpeed/commits/main/">
    <img alt="Last Commit" src="https://img.shields.io/github/last-commit/szejkerek/LethalSpeed?logo=Mapbox&color=orange">
  </a>
  <a>
    <img alt="Repo Size" src="https://img.shields.io/github/repo-size/szejkerek/LethalSpeed?logo=VirtualBox">
  </a>
  <a href="https://github.com/szejkerek/LethalSpeed/releases">
    <img alt="GitHub Release" src="https://img.shields.io/github/v/release/szejkerek/LethalSpeed">
  </a>
  <a>
    <img alt="GitHub stars" src="https://img.shields.io/github/stars/szejkerek/LethalSpeed?branch=main&label=Stars&logo=GitHub&logoColor=ffffff&labelColor=282828&color=informational&style=flat">
  </a>
  <a>
    <img alt="GitHub user stars" src="https://img.shields.io/github/stars/szejkerek?affiliations=OWNER&branch=main&label=User%20Stars&logo=GitHub&logoColor=ffffff&labelColor=282828&color=informational&style=flat">
  </a>
</p>
    
# Introduction

Engage in a fast-paced game where your adept movement skills are crucial for swiftly dispatching foes with a sword, all while navigating intense parkour challenges to conquer three exhilarating levels.
    
[![YTLINK](https://github.com/szejkerek/LethalSpeed/assets/69083596/316fd34e-d6bb-458c-a354-4869b8479c0e)](https://youtu.be/0ZiTxyrfNDY?si=UzJZH7nFuwDes1x3)    

# Gameplay
The gameplay consists of three distinct maps, and players have the flexibility to choose the order in which they engage with each map. The primary goal is to achieve the best possible completion time while eliminating all enemies present on each map.
- Each map is designed with unique challenges and layouts
- Time is the primary metric for scoring
- The non-linear nature of map selection enhances replayability and strategic planning

![zhsxCn](https://github.com/szejkerek/LethalSpeed/assets/69083596/5836f692-ad4d-434e-88b6-99e2efb20644)

![hy3dif](https://github.com/szejkerek/LethalSpeed/assets/69083596/c4918ac7-1eaf-4e53-9809-b0674851429b)

![MAeeLZ](https://github.com/szejkerek/LethalSpeed/assets/69083596/791b00ba-3aa6-4c82-b104-67b24492ba31)

![UT_pMW](https://github.com/szejkerek/LethalSpeed/assets/69083596/8dd71a21-a380-42f9-ac50-500915b6e857)

# Downloading the Game

To get started and play the game, you can follow these simple steps to download the latest release from GitHub.

- Visit the [Releases](https://github.com/szejkerek/LethalSpeed/releases) page of our GitHub repository.
- Look for the latest release.
- Download the appropriate asset for your operating system.
- Follow the installation instructions for your specific operating system. After installation, you should be able to launch the game and start playing!


# Controls

**W**: Move forward

**A**: Move left

**S**: Move backward

**D**: Move right

**Space**: Jump

**Space near wall in air**: Wallrun

**Shift**: Dash

**Ctrl**: Crouch

**Q**: Deploy Orange Point Grappling Hook

**Right Mouse Button (RMB)**: Initiate Blue Point Swing

**Left Mouse Button (LMB)**: Swing sword

**R**: Reset level

## Code Highlights

### Multi-Point Occluded Vision System

```csharp
// Assets/__Scripts/Enemy/VisionEnemyAI.cs
private void UpdatePlayerBodyPartsPositions(Transform scannedPlayer)
{
    float scaledPlayerHeight = playerHeight * playerMovment.transform.localScale.y;

    middlePosition = scannedPlayer.position;
    topPosition = scannedPlayer.position + Vector3.up * (scaledPlayerHeight / 2 - _topError);
    bottomPosition = scannedPlayer.position + Vector3.down * (scaledPlayerHeight / 2 - _topError);
    rightPosition = scannedPlayer.position + -Vector3.Cross(Vector3.up, transform.position - scannedPlayer.position).normalized * _sideError;
    leftPosition = scannedPlayer.position + Vector3.Cross(Vector3.up, transform.position - scannedPlayer.position).normalized * _sideError;

    Vector3 myForward = -(scannedPlayer.position - transform.position).normalized;
    myForward.y = 0;

    forwardPosition = scannedPlayer.position + myForward * _sideError;
    backPosition = scannedPlayer.position + -myForward * _sideError;
}

private bool IsInVision()
{
    isBlockedMiddle = Physics.Linecast(_eyeLevel.position, middlePosition, _blockers);
    isBlockedTop = Physics.Linecast(_eyeLevel.position, topPosition, _blockers);
    isBlockedBottom = Physics.Linecast(_eyeLevel.position, bottomPosition, _blockers);
    isBlockedLeft = Physics.Linecast(_eyeLevel.position, leftPosition, _blockers);
    isBlockedRight = Physics.Linecast(_eyeLevel.position, rightPosition, _blockers);
    isBlockedForward = Physics.Linecast(_eyeLevel.position, forwardPosition, _blockers);
    isBlockedBack = Physics.Linecast(_eyeLevel.position, backPosition, _blockers);

    bool seeCorePart = !isBlockedMiddle || !isBlockedTop || !isBlockedBottom;

    int sidePartsCount = 0;
    if (!isBlockedLeft) sidePartsCount++;
    if (!isBlockedRight) sidePartsCount++;
    if (!isBlockedForward) sidePartsCount++;
    if (!isBlockedBack) sidePartsCount++;

    return seeCorePart || sidePartsCount > 1;
}
```

Rather than a single center-of-mass raycast, the system samples seven distinct body-part positions — top, middle, bottom, left, right, forward, and back. Detection uses a voting rule: the enemy spots the player if any core part is visible, or if at least two side points are simultaneously unoccluded. Body-part positions are computed dynamically and account for the player's current scale, which changes during crouching and sliding. Scanning is throttled with `OverlapSphereNonAlloc` to avoid per-frame heap allocations, and a separate reaction-time timer delays the enemy's awareness after first sighting.

### Wall-Tangent Movement with Tiered Momentum Clipping

```csharp
// Assets/__Scripts/Player/Movement/WallruninngState.cs
public void Move(Vector3 normalizedWishDir)
{
    Vector3 forwardDir = Vector3.Cross(_wallNormal, Vector3.up);

    if((_pm.Orientation.forward - forwardDir).magnitude > (_pm.Orientation.forward + forwardDir).magnitude)
    {
        forwardDir = -forwardDir;
    }

    float wishedForwardDirMultiplier = Vector3.Dot(normalizedWishDir, forwardDir);

    _pm.Rigidbody.AddForce(wishedForwardDirMultiplier * forwardDir * _pm.Velocity.magnitude * _pm.WallrunProps.Acceleration, ForceMode.Force);
    _pm.Rigidbody.AddForce(Vector3.down * _pm.AirProps.GravityForce / 3.0f, ForceMode.Force);

    if(Input.GetKey(_pm.JumpKey))
    {
        _pm.Rigidbody.AddForce(Vector3.up * _pm.AirProps.JumpForce * 2.0f, ForceMode.Force);
    }
}

private void ClipWallrunSpeed()
{
    if (_initialSpeed > _pm.CurrentMaxSpeed)
    {
        float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 1.0f ? 
            _pm.GroundProps.Deacceleration * Time.deltaTime / 50.0f : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;

        Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialSpeed, _pm.FlatVelocity.magnitude - drop);

        _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
    }
    else if (_pm.FlatVelocity.magnitude > _pm.CurrentMaxSpeed)
    {
        float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 3.0f ? _pm.GroundProps.Deacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;
        Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

        _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
    }
}
```

`Move` derives the wall-parallel forward direction via `Vector3.Cross(_wallNormal, Vector3.up)`, then disambiguates its sign by comparing distances to the player's orientation forward — a compact way to pick the correct tangent direction without explicit left/right detection flags. A dot product of wish direction against this forward vector scales the applied force, allowing natural speed modulation when the player steers with or against the wall. `ClipWallrunSpeed` implements tiered deceleration: large overshoots decay gradually while small ones snap immediately to cap, making the speed feel organic rather than abruptly clamped.

### Procedural 3D Spring-Wave Rope Visualization

```csharp
// Assets/__Scripts/Player/RopeRenderer/RopeRenderer.cs
public void DrawRope(bool isGrappling, Vector3 end)
{
    if (lr.positionCount == 0)
    {
        currentGrapplePosition = transform.position;
        rope.Velocity = velocity;
        lr.positionCount = quality + 1;
    }

    rope.Damper = damper;
    rope.Strength = strength;
    rope.Update(Time.deltaTime);

    var grapplePoint = end;
    var gunTipPosition = transform.position;
    var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

    currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 12f);

    for (var i = 0; i < quality + 1; i++)
    {
        var delta = i / (float)quality;
        var right = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.right;

        var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * rope.Value *
                                 affectCurve.Evaluate(delta) +
                                 right * waveHeight * Mathf.Cos(delta * waveCount * Mathf.PI) * rope.Value *
                                 affectCurve.Evaluate(delta);

        lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
    }
}
```

Each rope segment is displaced along two perpendicular axes (up and right, derived from the rope's local frame via `Quaternion.LookRotation`) by sine and cosine waves with a 90-degree phase offset, producing a 3D helical undulation rather than a flat 2D swing. Wave amplitude is multiplied by `rope.Value`, the output of a scalar spring-damper simulation that starts high when the grapple fires and decays to zero as the rope settles — giving an organic whip effect without keyframed animation. An `AnimationCurve` further shapes the falloff along the rope's length, concentrating visual energy near the gun tip. The endpoint is Lerped toward the grapple target at a fixed rate to animate the launch extension.

