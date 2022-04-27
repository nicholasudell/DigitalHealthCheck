using System;

namespace DigitalHealthCheckWeb.Components.Pages
{
    public class Barrier : IEquatable<Barrier>
    {
      public string Text { get; set; }

      public string Value { get; set; }

      public bool Equals(Barrier other) =>
          other is not null &&
          string.Equals(other.Value, Value, StringComparison.OrdinalIgnoreCase);

      public override bool Equals(object obj) => obj is Barrier other && this.Equals(other);

      public override int GetHashCode() => Value.GetHashCode();
    }
}
