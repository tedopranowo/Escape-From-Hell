using UnityEngine;

public class Timer
{
    public enum Status
    {
        running,
        stopped,
        finished
    }
    /*[Tedo]:
     * m_startTime and m_endTime are not actually being used */
    float m_startTime = 0.0f;
    float m_endTime = 0.0f;
    float m_timerMultiplier = 1.0f;
    float m_runTime = 0.0f;
    float m_timeToRun = 0.0f;
    Status m_status = Status.stopped;
    public Status status { get { return m_status; } }

    public Timer(float timeToRun, float multiplier = 1)
    {
        m_startTime = Time.time;
        m_endTime = m_startTime + timeToRun;
        m_timeToRun = timeToRun;
        /*[Tedo]
         * m_timerMultiplier = multiplier */
    }

    public void StopTimer()
    {
        if(m_status != Status.finished)
            m_status = Status.stopped;
    }
    public void ResumeTimer()
    {
        if(m_status != Status.finished)
            m_status = Status.running;
    }

    public void Tick()
    {
        float deltaTime = Time.deltaTime;
        if (m_status == Status.running)
        {
            m_runTime += deltaTime * m_timerMultiplier;
            if(m_runTime >= m_timeToRun)
            {
                m_status = Status.finished;
            }
        }
    }

    public void ResetTimer(float timeToRun = 0.0f)
    {
        if (timeToRun != 0.0f)
        {
            m_timeToRun = timeToRun;
        }
        m_startTime = Time.time;
        m_endTime = m_startTime + m_timeToRun;

        m_status = Status.running;
        m_runTime = 0.0f;
    }
}
