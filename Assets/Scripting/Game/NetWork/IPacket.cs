using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IPacket{

    int     StatusCode { get; set; }
    short   NetCode { get; }

    void ReadFrom(BinaryReader ios);
    void WriteTo(BinaryWriter ios);

    // void ReadFrom(BinaryReader ios);
    // void WriteTo(BinaryWriter ios);
    // void Serialize(BinaryReader ios);

}
