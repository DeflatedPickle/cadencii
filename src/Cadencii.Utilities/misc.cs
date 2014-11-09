/*
 * misc.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.core.
 *
 * cadencii.core is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace Cadencii.Utilities
{

    public static class Misc
    {
        public static string getmd5(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.Unicode.GetBytes(s);
            byte[] hash = md5.ComputeHash(data);
            return BitConverter.ToString(hash).ToLower().Replace("-", "_");
        }

        public static string getmd5(FileStream file_stream)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(file_stream);
            return BitConverter.ToString(hash).ToLower().Replace("-", "_");
        }

        /// <summary>
        /// 現在の実行アセンブリで使用されている型のリストを取得します
        /// </summary>
        /// <returns></returns>
        public static Type[] get_executing_types()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            List<Type> types = new List<Type>(asm.GetTypes());
            foreach (System.Reflection.AssemblyName asmname in asm.GetReferencedAssemblies()) {
                System.Reflection.Assembly asmref = System.Reflection.Assembly.Load(asmname);
                types.AddRange(asmref.GetTypes());
            }

            asm = System.Reflection.Assembly.GetCallingAssembly();
            types.AddRange(asm.GetTypes());
            foreach (System.Reflection.AssemblyName asmname in asm.GetReferencedAssemblies()) {
                System.Reflection.Assembly asmref = System.Reflection.Assembly.Load(asmname);
                types.AddRange(asmref.GetTypes());
            }

            asm = System.Reflection.Assembly.GetEntryAssembly();
            types.AddRange(asm.GetTypes());
            foreach (System.Reflection.AssemblyName asmname in asm.GetReferencedAssemblies()) {
                System.Reflection.Assembly asmref = System.Reflection.Assembly.Load(asmname);
                types.AddRange(asmref.GetTypes());
            }

            List<Type> ret = new List<Type>();
            foreach (Type t in types) {
                if (t.IsPublic && !ret.Contains(t)) {
                    ret.Add(t);
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// 現在の実行アセンブリで使用されている名前空間のリストを取得します
        /// </summary>
        /// <returns></returns>
        public static string[] get_executing_namespaces()
        {
            Type[] types = get_executing_types();
            List<string> list = new List<string>();
            foreach (Type t in types) {
                if (!list.Contains(t.Namespace)) {
                    list.Add(t.Namespace);
                }
            }
            list.Sort();
            return list.ToArray();
        }
    }
}
