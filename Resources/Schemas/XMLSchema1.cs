﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.8.4084.0.
// 
namespace Resources.Schemas.XMLSchema1 {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/XMLSchema1.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd", IsNullable=false)]
    public partial class Root {
        
        private MapType mapField;
        
        private MapTilesType mapTilesField;
        
        private PlayerTilesType playerTilesField;
        
        private MapType controlField;
        
        private ControlTilesType controlTilesField;
        
        private AvailableControlTilesType availableControlTilesField;
        
        /// <remarks/>
        public MapType Map {
            get {
                return this.mapField;
            }
            set {
                this.mapField = value;
            }
        }
        
        /// <remarks/>
        public MapTilesType MapTiles {
            get {
                return this.mapTilesField;
            }
            set {
                this.mapTilesField = value;
            }
        }
        
        /// <remarks/>
        public PlayerTilesType PlayerTiles {
            get {
                return this.playerTilesField;
            }
            set {
                this.playerTilesField = value;
            }
        }
        
        /// <remarks/>
        public MapType Control {
            get {
                return this.controlField;
            }
            set {
                this.controlField = value;
            }
        }
        
        /// <remarks/>
        public ControlTilesType ControlTiles {
            get {
                return this.controlTilesField;
            }
            set {
                this.controlTilesField = value;
            }
        }
        
        /// <remarks/>
        public AvailableControlTilesType AvailableControlTiles {
            get {
                return this.availableControlTilesField;
            }
            set {
                this.availableControlTilesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class MapType {
        
        private string nameField;
        
        private uint widthField;
        
        private uint heightField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public uint Width {
            get {
                return this.widthField;
            }
            set {
                this.widthField = value;
            }
        }
        
        /// <remarks/>
        public uint Height {
            get {
                return this.heightField;
            }
            set {
                this.heightField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class AvailableTileType {
        
        private object itemField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Color", typeof(TileColorType))]
        [System.Xml.Serialization.XmlElementAttribute("Sign", typeof(uint))]
        public object Item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public enum TileColorType {
        
        /// <remarks/>
        gray,
        
        /// <remarks/>
        green,
        
        /// <remarks/>
        red,
        
        /// <remarks/>
        blue,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class AvailableControlTilesType {
        
        private AvailableTileType[] availableControlTileField;
        
        private uint idField;
        
        private bool idFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AvailableControlTile")]
        public AvailableTileType[] AvailableControlTile {
            get {
                return this.availableControlTileField;
            }
            set {
                this.availableControlTileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified {
            get {
                return this.idFieldSpecified;
            }
            set {
                this.idFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class ControlTileType {
        
        private uint lengthField;
        
        /// <remarks/>
        public uint Length {
            get {
                return this.lengthField;
            }
            set {
                this.lengthField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class ControlTilesType {
        
        private ControlTileType[] controlTileField;
        
        private uint idField;
        
        private bool idFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ControlTile")]
        public ControlTileType[] ControlTile {
            get {
                return this.controlTileField;
            }
            set {
                this.controlTileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified {
            get {
                return this.idFieldSpecified;
            }
            set {
                this.idFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class PlayerTilesType {
        
        private TileType playerTileField;
        
        private uint idField;
        
        private bool idFieldSpecified;
        
        /// <remarks/>
        public TileType PlayerTile {
            get {
                return this.playerTileField;
            }
            set {
                this.playerTileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified {
            get {
                return this.idFieldSpecified;
            }
            set {
                this.idFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class TileType {
        
        private uint xField;
        
        private uint yField;
        
        private TileColorType colorField;
        
        /// <remarks/>
        public uint X {
            get {
                return this.xField;
            }
            set {
                this.xField = value;
            }
        }
        
        /// <remarks/>
        public uint Y {
            get {
                return this.yField;
            }
            set {
                this.yField = value;
            }
        }
        
        /// <remarks/>
        public TileColorType Color {
            get {
                return this.colorField;
            }
            set {
                this.colorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/XMLSchema1.xsd")]
    public partial class MapTilesType {
        
        private TileType[] mapTileField;
        
        private uint idField;
        
        private bool idFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MapTile")]
        public TileType[] MapTile {
            get {
                return this.mapTileField;
            }
            set {
                this.mapTileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified {
            get {
                return this.idFieldSpecified;
            }
            set {
                this.idFieldSpecified = value;
            }
        }
    }
}
