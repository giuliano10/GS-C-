namespace CyberImpactMonitor.Models
{
    public class Equipamento
    {
        public string Nome { get; protected set; }
        public string Status { get; protected set; }

        public Equipamento(string nome)
        {
            Nome = nome;
            Status = "Operacional";
        }

        public virtual void MarcarFalha()
        {
            Status = "Falha";
        }

        public virtual void Restaurar()
        {
            Status = "Operacional";
        }
    }
}
