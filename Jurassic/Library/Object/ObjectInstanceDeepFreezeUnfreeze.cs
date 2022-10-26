namespace Jurassic.Library
{
    public partial class ObjectInstance
    {
        public void DeepUnfreeze()
        {
            RecursiveUnfreeze(this);
        }

        public void DeepFreeze()
        {
            RecursiveDeepFreeze(this);
        }

        private void RecursiveUnfreeze(ObjectInstance objectInstance)
        {
            objectInstance.flags = ObjectFlags.Extensible;
            
            foreach (var key in objectInstance.OwnKeys)
            {
                var descriptor = objectInstance.GetOwnPropertyDescriptor(key);

                if (descriptor.Value is ObjectInstance valueAsObjectInstance)
                {
                    RecursiveUnfreeze(valueAsObjectInstance);
                    continue;
                }
                
                objectInstance.UnfreezeProperty(key);
            }
        }

        private void RecursiveDeepFreeze(ObjectInstance objectInstance)
        {
            objectInstance.flags &= ~ObjectFlags.Extensible;
            
            foreach (var key in objectInstance.OwnKeys)
            {
                var descriptor = objectInstance.GetOwnPropertyDescriptor(key);

                if (descriptor.Value is ObjectInstance valueAsObjectInstance)
                {
                    RecursiveDeepFreeze(valueAsObjectInstance);
                    continue;
                }
                
                objectInstance.FreezeProperty(key);
            }
        }
        
        private void FreezeProperty(object key)
        {
            // Retrieve info on the property.
            var current = this.schema.GetPropertyIndexAndAttributes(key);

            if (current.Exists == false)
            {
                return;
            }
            
            //Frozen properties are ONLY enumerable
            this.schema = this.schema.SetPropertyAttributes(key,PropertyAttributes.Enumerable);
        }

        //ObjectInstance.DefineProperty will block updates to a frozen object. This method bypasses the check and implements
        //the same logic. 
        private void UnfreezeProperty(object key)
        {
            // Retrieve info on the property.
            var current = this.schema.GetPropertyIndexAndAttributes(key);

            if (current.Exists == false)
            {
                return;
            }
            
            this.schema = this.schema.SetPropertyAttributes(key,PropertyAttributes.FullAccess);
        }
    }
}