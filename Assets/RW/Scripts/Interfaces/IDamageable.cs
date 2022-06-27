namespace RayWenderlich.Unity.StatePatternInUnity.Interfaces
{
    public interface IDamageable
    {
        // interface that is extended by entities that are damageable.
        
        public int Health { get; set; }

        public void Damage(); // method to override different damage behaviors.
    }
}