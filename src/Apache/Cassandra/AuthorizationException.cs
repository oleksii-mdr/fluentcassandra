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

  /// <summary>
  /// invalid authorization request (user does not have access to keyspace)
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class AuthorizationException : Exception, TBase
  {
    private string _why;

    public string Why
    {
      get
      {
        return _why;
      }
      set
      {
        __isset.why = true;
        this._why = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool why;
    }

    public AuthorizationException() {
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
              Why = iprot.ReadString();
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
      TStruct struc = new TStruct("AuthorizationException");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Why != null && __isset.why) {
        field.Name = "why";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Why);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("AuthorizationException(");
      sb.Append("Why: ");
      sb.Append(Why);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
