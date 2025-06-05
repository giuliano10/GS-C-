namespace CyberImpactMonitor.Models
{
    public class Subestacao : Equipamento
    {
        public bool AlertaCritico { get; private set; }

        public Subestacao(string nome) : base(nome)
        {
            AlertaCritico = false;
        }

        public override void MarcarFalha()
        {
            base.MarcarFalha();
            AlertaCritico = true;
        }

        public override void Restaurar()
        {
            base.Restaurar();
            AlertaCritico = false;
        }
    }
}
