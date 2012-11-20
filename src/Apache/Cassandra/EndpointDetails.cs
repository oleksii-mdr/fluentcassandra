/**
 * Autogenerated by Thrift Compiler (0.9.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FluentCassandra.Thrift;
using FluentCassandra.Thrift.Collections;
using System.Runtime.Serialization;
using FluentCassandra.Thrift.Protocol;
using FluentCassandra.Thrift.Transport;

namespace FluentCassandra.Apache.Cassandra
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class EndpointDetails : TBase
  {
    private string _host;
    private string _datacenter;
    private string _rack;

    public string Host
    {
      get
      {
        return _host;
      }
      set
      {
        __isset.host = true;
        this._host = value;
      }
    }

    public string Datacenter
    {
      get
      {
        return _datacenter;
      }
      set
      {
        __isset.datacenter = true;
        this._datacenter = value;
      }
    }

    public string Rack
    {
      get
      {
        return _rack;
      }
      set
      {
        __isset.rack = true;
        this._rack = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool host;
      public bool datacenter;
      public bool rack;
    }

    public EndpointDetails() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              Host = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Datacenter = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Rack = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("EndpointDetails");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Host != null && __isset.host) {
        field.Name = "host";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Host);
        oprot.WriteFieldEnd();
      }
      if (Datacenter != null && __isset.datacenter) {
        field.Name = "datacenter";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Datacenter);
        oprot.WriteFieldEnd();
      }
      if (Rack != null && __isset.rack) {
        field.Name = "rack";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Rack);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("EndpointDetails(");
      sb.Append("Host: ");
      sb.Append(Host);
      sb.Append(",Datacenter: ");
      sb.Append(Datacenter);
      sb.Append(",Rack: ");
      sb.Append(Rack);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
