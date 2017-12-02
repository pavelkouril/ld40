using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daytime : MonoBehaviour
{
    public const int kSecondsInDay = 60 * 60 * 24;

    public const int kDaysInMonth = 30;

    public const int kMonthsInYear = 12;

    public const float kRealSecondsInDay = 15;

    public static float DeltaTime
    {
        get
        {
            return ((Time.deltaTime / kRealSecondsInDay) * kSecondsInDay) / kSecondsInDay;
        }
    }

    public float TimeOfDayUtc { get; set; }

    public float DayOfMonth { get; set; }

    public float Month { get; set; }

    private void Start()
    {
        TimeOfDayUtc = 0.5f;
        DayOfMonth = 1;
        Month = 5;
        DebugDate();
    }

    private void Update()
    {
        TimeOfDayUtc += DeltaTime;
        if (TimeOfDayUtc > 1)
        {
            TimeOfDayUtc -= 1;
            DayOfMonth++;
            if (DayOfMonth > kDaysInMonth)
            {
                DayOfMonth = 1;
                Month++;
                if (Month > kMonthsInYear)
                {
                    Month = 1;
                }
            }
            DebugDate();
        }
    }

    private void DebugDate()
    {
        Debug.LogFormat("Day: {0}, Month: {1}", DayOfMonth, Month);
    }
}
