#region License
//
// Copyright 2002-2019 Drew Noakes
// Ported from Java to C# by Yakov Danilov for Imazen LLC in 2014
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// More information about this project is available at:
//
//    https://github.com/drewnoakes/metadata-extractor-dotnet
//    https://drewnoakes.com/code/exif/
//
#endregion

using System.Collections.Generic;
using System.IO;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.Riff;
using MetadataExtractor.IO;

#if NET35
using DirectoryList = System.Collections.Generic.IList<MetadataExtractor.Directory>;
#else
using DirectoryList = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;
#endif

namespace MetadataExtractor.Formats.Wav
{
    /// <summary>Obtains metadata from WAV files.</summary>
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public static class WavMetadataReader
    {
        /// <exception cref="IOException"/>
        /// <exception cref="RiffProcessingException"/>
        public static DirectoryList ReadMetadata(string filePath)
        {
            var directories = new List<Directory>();

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                directories.AddRange(ReadMetadata(stream));

            directories.Add(new FileMetadataReader().Read(filePath));

            return directories;
        }

        /// <exception cref="IOException"/>
        /// <exception cref="RiffProcessingException"/>
        public static DirectoryList ReadMetadata(Stream stream)
        {
            var directories = new List<Directory>();
            new RiffReader().ProcessRiff(new SequentialStreamReader(stream), new WavRiffHandler(directories));
            return directories;
        }
    }
}