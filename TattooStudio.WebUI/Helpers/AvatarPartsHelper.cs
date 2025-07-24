namespace TattooStudio.WebUI.Helpers
{
    public static class AvatarPartsHelper
    {
        public static List<string> GetFrontParts()
        {
            return new List<string>
            {
                "Cabeça (Frente)", "Pescoço (Frente)",
                "Peito Esquerdo", "Peito Direito",
                "Costela Esquerda", "Costela Direita",
                "Braço Esquerdo", "Antebraço Esquerdo", "Mão Esquerda",
                "Braço Direito", "Antebraço Direito", "Mão Direita",
                "Perna Esquerda", "Perna Direita",
                "Pé Esquerdo", "Pé Direito"
            };
        }

        public static List<string> GetBackParts()
        {
            return new List<string>
            {
                "Cabeça (Costas)", "Nuca",
                "Costas (Ombro) Esquerdo", "Costas (Ombro) Direito",
                "Costas (Cintura) Esquerda", "Costas (Cintura) Direita",
                "Braço Esquerdo (Costas)", "Antebraço Esquerdo (Costas)", "Mão Esquerda (Costas)",
                "Braço Direito (Costas)", "Antebraço Direito (Costas)", "Mão Direita (Costas)",
                "Perna Esquerda (Costas)", "Perna Direita (Costas)",
                "Pé Esquerdo (Costas)", "Pé Direito (Costas)"
            };
        }

        public static List<string> GetAllParts()
        {
            var allParts = new List<string>();
            allParts.AddRange(GetFrontParts());
            allParts.AddRange(GetBackParts());
            return allParts.Distinct().OrderBy(p => p).ToList();
        }
    }
}