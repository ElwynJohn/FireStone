using UnityEngine;

namespace Firestone.ParticleSystems
{
    //TODO make reusable
    public class SmokePS : MonoBehaviour
    {
        [SerializeField] ParticleSystem dustPS = default;

        private void TriggerDustPS()
        {
            //create a position for the particle systems and instantiate them at this position
            var psPosition = gameObject.transform.position;
            psPosition.x += Random.Range(-0.25f, 0f);
            ParticleSystem dustPS = Instantiate(this.dustPS, psPosition, Quaternion.identity,
                gameObject.transform) as ParticleSystem;

            ParticleSystem dustPS1 = Instantiate(this.dustPS, gameObject.transform.position, Quaternion.identity,
                gameObject.transform);

            var psPosition2 = gameObject.transform.position;
            psPosition2.x += Random.Range(0f, 0.25f);
            ParticleSystem dustPS2 = Instantiate(this.dustPS, psPosition2, Quaternion.identity,
                gameObject.transform);


            //set velocity of particle systems and play them
            var velocity = dustPS.velocityOverLifetime;
            velocity.xMultiplier = Random.Range(-0.5f, -1f);
            dustPS.Play();

            dustPS1.Play();

            var velocity2 = dustPS2.velocityOverLifetime;
            velocity2.xMultiplier = Random.Range(0.1f, 1f); ;
            dustPS2.Play();

            //Destroy Particle Systems to clean up hierarchy
            Destroy(dustPS, 3f);
            Destroy(dustPS1, 3f);
            Destroy(dustPS2, 3f);
        }
    }
}