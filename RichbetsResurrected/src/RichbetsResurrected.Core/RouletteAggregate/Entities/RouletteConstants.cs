﻿namespace RichbetsResurrected.Core.RouletteAggregate.Entities;

public static class RouletteConstants
{
    public const int TotalSegments = 37;

    private static readonly int[] NumberToSegment =
    {
        37, 23, 6, 35, 4, 19, 10, 31, 16, 27, 18,
        14, 33, 12, 25, 2, 21, 8, 29, 3, 24,
        5, 28, 17, 20, 7, 36, 11, 32, 30, 15,
        26, 1, 22, 9, 34, 13
    };
    public static double GetRandomAngleForSegment(int segment, int totalSegments)
    {
        const int totalAngles = 360;
        var radiusOfSegment = (double)totalAngles / totalSegments;
        var startAngle = radiusOfSegment * segment - radiusOfSegment;
        var endAngle = startAngle + radiusOfSegment;
        var range = endAngle - startAngle - 2;
        return startAngle + 1 + Math.Floor(Random.Shared.NextDouble() * range);
    }
    
    public static int GetSegmentForNumber(int number)
    {
        return NumberToSegment[number];
    }
}