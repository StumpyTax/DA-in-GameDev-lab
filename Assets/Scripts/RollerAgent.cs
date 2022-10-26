using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent
{
  Rigidbody rBody;
  // Start is called before the first frame update
  void Start()
  {
    rBody=GetComponent<Rigidbody>();
  }
  public Transform Target;
  public Transform Target2;
  public bool firstOrSecond;
  public float targetsDistance=8;
  public override void OnEpisodeBegin()
  {
    if (this.transform.localPosition.y<0){
      this.rBody.angularVelocity=Vector3.zero;
      this.rBody.velocity=Vector3.zero;
      this.transform.localPosition=new Vector3(0,0.5f,0);
    }

    Target.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
    Target2.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
    targetsDistance=Vector3.Distance(Target.localPosition,Target2.localPosition);
    if (!(targetsDistance>3 && targetsDistance<7))
      while(!(targetsDistance>3 && targetsDistance<7)){
       Target.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
        Target2.localPosition=new Vector3(Random.value*8-4,0.5f,Random.value*8-4);
        targetsDistance=Vector3.Distance(Target.localPosition,Target2.localPosition);
      }
  }

  public override void CollectObservations(VectorSensor sensor)
  {
    sensor.AddObservation(Target.localPosition);
    sensor.AddObservation(Target2.localPosition);
    sensor.AddObservation(this.transform.localPosition);
    sensor.AddObservation(rBody.velocity.x);
    sensor.AddObservation(rBody.velocity.z);
  }
  public float forceMultiplier=10;
  public override void OnActionReceived(ActionBuffers actionBuffers)
  {
    Vector3 controlSignal =Vector3.zero;
    controlSignal.x=actionBuffers.ContinuousActions[0];
    controlSignal.z=actionBuffers.ContinuousActions[1];
    rBody.AddForce(controlSignal*forceMultiplier);

    float distanceToTarget=Vector3.Distance(this.transform.localPosition,Target.localPosition);
    float distanceToTarget2=Vector3.Distance(this.transform.localPosition,Target2.localPosition);
    if(distanceToTarget+distanceToTarget2==targetsDistance){
      SetReward(1.0f);
      EndEpisode();
    }
    else if (this.transform.localPosition.y<0){
      EndEpisode();
    }

  }
}
