﻿//Owner: Lousada
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// The MagneticForce script is used to allow objects to interact with one another on a magnetic level
/// each object using this script must be on the magnetic layer and have a trigger collider
/// </summary>
public class MagneticForce : MonoBehaviour, Activator{
    #region MagneticForce Constants

    private const float DUMMY_DISTANCE = 2.0f;
    private const float DUMMY_FORCE = 10.0f;

    //ToDo - Needs fine tuning // Change these 3 variables to const
    [SerializeField]
    private float LOW_FORCE_FACTOR = 2.0f;
    [SerializeField]
    private float MEDIUM_FORCE_FACTOR = 20.0f;
    [SerializeField]
    private float HIGH_FORCE_FACTOR = 100.0f;

    #endregion

    #region MagneticForce Variables
    [SerializeField]
    private bool _isActivated = true;

    public enum Force { LOW, MEDIUM, HIGH };
    public enum Charge { NEGATIVE, POSITIVE};
    [SerializeField]
    private Force _force = Force.MEDIUM;
    [SerializeField]
    private Charge _charge = Charge.NEGATIVE;

    private List<MagneticForce> _affectingMagnets = new List<MagneticForce>();
    #endregion

    #region MagneticForce Properties

    public Force force{
        get { return _force; }
    }

    public Charge charge{
        get { return _charge; }
    }

    public bool isActivated{
        get { return _isActivated; }
    }

    public List<MagneticForce> affectingMagnets {
        get { return _affectingMagnets; }
    }

    public float low_force_factor {
        get { return LOW_FORCE_FACTOR; }
    }

    public float medium_force_factor {
        get { return MEDIUM_FORCE_FACTOR; }
    }

    public float high_force_factor {
        get { return HIGH_FORCE_FACTOR; }
    }
    #endregion


   void Activator.Activate(){
        _isActivated = true;
    }

    void Activator.Deactivate(){
        _isActivated = false;
    }

    /// <summary>
    /// The AffectedBy function makes the MagneticForce provided begin to influence this object
    /// </summary>
    public void AffectedBy(MagneticForce otherMagnet)
    {
     _affectingMagnets.Add(otherMagnet);
    }

    /// <summary>
    /// Stops the influence of the MagneticForce provided
    /// </summary>
    public void NoLongerAffectedBy(MagneticForce otherMagnet) {
        foreach (MagneticForce m in _affectingMagnets) {
            float aux = Vector3.Distance(otherMagnet.transform.position, m.transform.position);
            if (aux < 0.5f) {
                _affectingMagnets.Remove(m);
                break;
            }

        }
    }


    public void OnTriggerEnter(Collider other){
        MagneticForce otherMagnet = (MagneticForce) other.gameObject.GetComponent("MagneticForce");
        AffectedBy(otherMagnet);
    }

    public void OnTriggerExit(Collider other) {
        MagneticForce otherMagnet = (MagneticForce)other.gameObject.GetComponent("MagneticForce");
        NoLongerAffectedBy(otherMagnet);
    }


    void Update(){
        ApplyOtherMagnetsForces();
    }

    /// <summary>
    /// Applies the influence other objects have over this one
    /// </summary>
    public virtual void ApplyOtherMagnetsForces() {
        foreach (MagneticForce otherMagnet in _affectingMagnets) {
            if (_isActivated && otherMagnet.isActivated) {
                Vector3 forceDirection = otherMagnet.transform.position - this.transform.position;
                float distance = Vector3.Distance(otherMagnet.transform.position, this.transform.position);
                float forceFactor = 0.0f;
                float totalForce = 0.0f;
                if (otherMagnet.charge == this.charge) {
                    forceDirection = (-1) * forceDirection;
                }

                if (distance < DUMMY_DISTANCE) {   //This is used to prevent object with different forces to push each other after colliding
                    forceFactor = DUMMY_FORCE;
                }
                else {
                    switch (force) {
                        case Force.LOW:
                            forceFactor = LOW_FORCE_FACTOR;
                            break;
                        case Force.MEDIUM:
                            forceFactor = MEDIUM_FORCE_FACTOR;
                            break;
                        case Force.HIGH:
                            forceFactor = HIGH_FORCE_FACTOR;
                            break;
                        default:
                            //throw exception perhaps
                            break;
                    }
                }  
                totalForce = (1 / (distance * distance ) * forceFactor);
                this.transform.parent.rigidbody.AddForce(totalForce * forceDirection);
            }

        }

    }

}
