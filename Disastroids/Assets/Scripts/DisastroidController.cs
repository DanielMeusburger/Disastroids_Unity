using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class DisastroidController : Controller
    {
        private bool _fireCommandRegistered = false;
        public bool FireCommandRegistered
        {
            get
            {
                bool returnValue = _fireCommandRegistered;
                _fireCommandRegistered = false;
                return returnValue;
            }
            set
            {
                _fireCommandRegistered = value;
            }
        }

        private bool _chargeCommnandRegistered = false;
        public bool ChargeCommandRegistered
        {
            get
            {
                bool returnvalue = _chargeCommnandRegistered;
                _chargeCommnandRegistered = false;
                return returnvalue;
            }
            set
            {
                _chargeCommnandRegistered = value;
            }
        }

        public DisastroidController()
        {
            actions["Fire"] = CommandFire;
            actions["Swipe"] = CommandCharge;
        }

        public void CommandFire(float x, float y, float z)
        {
            FireCommandRegistered = true;
        }

        public void CommandCharge(float x, float y, float z)
        {
            ChargeCommandRegistered = true;
        }
    }
}
