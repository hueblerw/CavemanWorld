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

}
