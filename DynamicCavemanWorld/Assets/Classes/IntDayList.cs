// No using yet

public class IntDayList {

    public int[] days = new int[120];

    public IntDayList(int[] array)
    {
        this.days = array;
    }

    public int getDaysTemp(int day)
    {
        return this.days[day];
    }

    public int Count32DegreeDays()
    {
        int count = 0;
        for (int day = 0; day < 120; day++)
        {
            if(days[day] <= 32)
            {
                count++;
            }
        }
        return count;
    }

    public int Count70DegreeDays()
    {
        int count = 0;
        for (int day = 0; day < 120; day++)
        {
            if (days[day] > 70)
            {
                count++;
            }
        }
        return count;
    }

}
