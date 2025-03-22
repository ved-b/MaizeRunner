using DG.Tweening;

public class CameraShake : SingletonMonoBehavior<CameraShake>
{
    
        public static void Shake(float duration, float strength)
        {
            Instance.OnShake(duration, strength);
        }

        private void OnShake(float duration, float strength)
        {
            transform.DOShakePosition(duration, strength);
            transform.DOShakeRotation(duration, strength);
        }
}
