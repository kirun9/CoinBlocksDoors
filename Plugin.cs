using EXILED;
using Harmony;
using System.Collections.Generic;
using MEC;
using System;

namespace CoinBlocksDoors
{
    public class Plugin : EXILED.Plugin
    {
        public override string getName { get; } = "Coin Block Doors";
        public IEnumerable<MEC.CoroutineHandle> Coroutines;
        public static List<DoorItem> Doors = new List<DoorItem>();

        public static readonly int MinUses = Config.GetInt("cbd_min_use", -1);
        public static readonly int MaxUses = Config.GetInt("cbd_max_use", -1);
        public static readonly float MinTime = Config.GetFloat("cbd_min_time", 5f);
        public static readonly float MaxTime = Config.GetFloat("cbd_max_time", 20f);
        public static readonly bool Enabled = Config.GetBool("cbd_enable", true);
        public static readonly bool AllowCheckpoint = Config.GetBool("cbd_allow_checkpoint", true) && false; // Do NOT set to true - it's buggy AF

        public override void OnEnable()
        {
            if (Config.GetBool("cbd_enable", true))
            {
                Events.DoorInteractEvent += OnDoorInteract;
            }
        }

        private void OnDoorInteract(ref DoorInteractionEvent ev)
        {
            try
            {
                var inv = ev.Player.inventory;
                var item = inv.GetItemInHand().id;
                if (string.Equals(ev.Door.permissionLevel, "CHCKPOINT_ACC", StringComparison.OrdinalIgnoreCase) && !AllowCheckpoint)
                {
                    return;
                }

                if (item == ItemType.Coin && MaxUses == 0)
                {
                    Coroutines.Add(Timing.RunCoroutine(LockDoor(ev.Door), Segment.Update));
                    if (!ev.Door.locked)
                        inv.items.RemoveAt(inv.GetItemIndex());
                    ev.Allow = false;
                    return;
                }

                if (Doors.Contains(new DoorItem(ev.Door, 0)))
                {
                    var temp = ev.Door;
                    var door = Doors.Find((d) => d.Door.GetInstanceID() == temp.GetInstanceID());
                    door.Used++;
                    if (door.Used == door.MaxUses)
                    {
                        Doors.Remove(door);
                    }
                    else
                    {
                        ev.Allow = false;
                    }
                }
                else if (item == ItemType.Coin)
                {
                    var random = (MinUses > 0) ? (MinUses < MaxUses) ? UnityEngine.Random.Range(MinUses, MaxUses) : (MinUses == MaxUses) ? MinUses : MinUses : 1;
                    var door = new DoorItem(ev.Door, random);
                    Doors.Add(door);
                    inv.items.RemoveAt(inv.GetItemIndex());
                    ev.Allow = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        private IEnumerator<float> LockDoor(Door door)
        {
            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitUntilFalse(() => door.locked);
            door.SetLock(true);
            yield return Timing.WaitForSeconds(RandomGenerator.GetFloat(MinTime, MaxTime));
            door.SetLock(false);
        }

        public override void OnDisable()
        {
            if (Enabled)
            {
                Events.DoorInteractEvent -= OnDoorInteract;
            }
        }

        public override void OnReload()
        {

        }
    }
    public class DoorItem: IEquatable<DoorItem>
    {
        public Door Door { get; set; }
        private int maxUses = 0;
        public int MaxUses
        {
            get
            {
                return maxUses;
            }
            set
            {
                maxUses = value;
                Door.SetLock(maxUses > used);
            }
        }
        private int used = 0;
        public int Used
        { 
            get
            {
                return used;
            }
            set
            {
                used = value;
                Door.SetLock(maxUses > used);
            }
        }

        public DoorItem(Door door, int maxUses)
        {
            Door = door;
            Used = 0;
            MaxUses = maxUses;
        }

        public bool Equals(DoorItem obj)
        {
            return obj.Door.GetInstanceID() == Door.GetInstanceID();
        }
    }
}
