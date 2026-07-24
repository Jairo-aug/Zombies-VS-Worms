public static class RNG {
    public static bool RollChance100(float chance) {
        System.Random r = new System.Random();
        int randomNumber = r.Next(0, 101);

        return randomNumber < chance;
    }
}