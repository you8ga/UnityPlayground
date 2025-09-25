using UnityEngine;

public class SceneTimer
{
    private float timerDuration;
    private float m_currentTime = 0f;
    public bool IsCounting { get; private set; }
    public bool HasFinished => m_currentTime>= timerDuration;

    public SceneTimer(float duration, bool startCounting = true)
    {
        timerDuration = duration;
        IsCounting = startCounting;
    }
    public void RenewTimer(float duration)
    {
        timerDuration = duration;
        m_currentTime = 0;
    }

    public void UpdateTime()
    {
        if (IsCounting && !HasFinished)
            m_currentTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        m_currentTime = 0f;
        IsCounting = true;
        Debug.Log($"Start Timer, Current Time = {m_currentTime}/ {timerDuration}");
    }

    public void PauseTimer()
    {
        IsCounting = false;
        Debug.Log($"Pause Timer, Current Time = {m_currentTime}/ {timerDuration}");
    }

    public void ResumeTimer()
    {
        if (!HasFinished)
        {
            IsCounting = true;
            Debug.Log($"Resume Timer, Current Time = {m_currentTime}/ {timerDuration}");
        }
    }
}
