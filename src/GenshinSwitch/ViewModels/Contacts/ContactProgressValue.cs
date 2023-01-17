namespace GenshinSwitch.ViewModels.Contacts;

public struct ContactProgressValue
{
    public double? ValueDouble { get; set; }
    public int? ValueInt32 { get; set; }

    public ContactProgressValue(double value)
    {
        ValueDouble = value;
    }

    public ContactProgressValue(int value)
    {
        ValueInt32 = value;
    }

    public static implicit operator ContactProgressValue(double value)
    {
        return new(value);
    }

    public static implicit operator double(ContactProgressValue self)
    {
        return self.ValueDouble ?? self.ValueInt32 ?? default;
    }

    public static implicit operator ContactProgressValue(int value)
    {
        return new(value);
    }

    public static implicit operator int(ContactProgressValue self)
    {
        return self.ValueInt32 ?? (int?)self.ValueDouble ?? default;
    }

    public override int GetHashCode()
    {
        return this;
    }

    public override string ToString()
    {
        if (ValueDouble != null)
        {
            return ValueDouble.ToString()!;
        }
        else if (ValueInt32 != null)
        {
            return ValueInt32.ToString()!;
        }
        return null!;
    }
}
