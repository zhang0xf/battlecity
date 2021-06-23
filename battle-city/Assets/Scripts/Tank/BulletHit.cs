using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_Bullet;
    [SerializeField] private ParticleSystem m_ExplosionParticles;
    [SerializeField] private AudioSource m_ExplosionAudio;

    private float m_LifeTime = 5f;
    private ParticleSystem.MainModule mainModule;

    private void Awake()
    {
        mainModule = m_ExplosionParticles.main;
        Destroy(gameObject, m_LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Barrier")
        {
            m_ExplosionParticles.transform.parent = null;

            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();

            Destroy(m_ExplosionParticles.gameObject, mainModule.duration);
            Destroy(gameObject);
        }
        else if (other.tag == "Wall")
        {
            m_ExplosionParticles.transform.parent = null;

            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();

            // destroy obj
            Destroy(m_ExplosionParticles.gameObject, mainModule.duration);
            Destroy(gameObject);
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
