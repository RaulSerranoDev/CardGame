using UnityEngine;

namespace Imperio
{
    /// <summary>
    /// Comando que juega una carta de la mano en el battlefield
    /// </summary>
    public class PlayAMinionCommand : Command
    {
        //Carta a jugar
        private MinionCard cardInfo;

        //Jugador que invoca la carta
        private Player player;

        private int tablePos;

        /// <summary>
        /// Comando que juega una carta de la mano en el battlefield
        /// </summary>
        public PlayAMinionCommand(Player player,MinionCard cardInfo, int tablePos)
        {
            this.cardInfo = cardInfo;
            this.player = player;
            this.tablePos = tablePos;
        }

        /// <summary>
        /// Empieza la reproducción del comando: juega un minion
        /// Instancia una carta del tipo adecuado, la mete en battlefield
        /// </summary>
        public override void CommandStart()
        {
            //Instanciamos el prefab del minion en la posición correspondiente
            GameObject GOminion = GameObject.Instantiate(GameManager.Instance.MinionPrefab, player.PlayerBattlefield.transform);
            GOminion.transform.SetSiblingIndex(tablePos);

            //Crear un nuevo minion y lo añadimos al Battlefield
            Minion newMinion = GOminion.GetComponent<Minion>();
            newMinion.BuildMinion(cardInfo);
            newMinion.Owner = player;

            player.PlayerBattlefield.PlayMinion(newMinion, tablePos);

            //TODO: Permitir previews??
            CommandComplete();
        }

    }
}