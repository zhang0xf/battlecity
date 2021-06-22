using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_Bullet;
    [SerializeField] private ParticleSystem m_ExplosionParticles;
    [SerializeField] private AudioSource m_ExplosionAudio;

    private ParticleSystem.MainModule mainModule;

    private void Awake()
    {
        mainModule = m_ExplosionParticles.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Barrier")
        {

            m_ExplosionParticles.transform.parent = null;

            // play particle
            m_ExplosionParticles.Play();

            // play audio
            m_ExplosionAudio.Play();

            // destroy particle
            Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

            // destroy bullet
            gameObject.SetActive(false);
        }
        else if (other.tag == "Wall")
        {
            m_ExplosionParticles.transform.parent = null;

            // play particle
            m_ExplosionParticles.Play();

            // play audio
            m_ExplosionAudio.Play();

            // destroy particle
            Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

            // destroy bullet
            gameObject.SetActive(false);

            // destroy wall
            Destroy(other.gameObject);
        }
        else if (other.tag == "Enemy")
        {

        }
        else if (other.tag == "Home")
        {
            GameManager.Instance.ChangeState(GameState.GAMEOVER);
        }
    }
}
