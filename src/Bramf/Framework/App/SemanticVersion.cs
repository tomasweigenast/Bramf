using Bramf.Extensions;
using System;
using System.Globalization;

namespace Bramf.App
{
    /// <summary>
    /// Represents the version number of an assembly, operating system, or the common 
    /// language runtime. This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class SemanticVersion : IEquatable<SemanticVersion>
    {
        #region Private Members

        private static readonly char[] SeparatorChars = new char[] { '.', '-' };

        private int mMajor;
        private int mMinor;
        private int mPatch;
        private string mLabel = Release;

        #endregion

        #region Default Version Labels

        /// <summary>
        /// Represents a Release version
        /// </summary>
        public const string Release = "release";

        /// <summary>
        /// Represents a Beta version
        /// </summary>
        public const string Beta = "beta";

        /// <summary>
        /// Represents an Alpha version
        /// </summary>
        public const string Alpha = "alpha";

        /// <summary>
        /// Represents a Pre-Release version
        /// </summary>
        public const string PreRelease = "prerelease";

        /// <summary>
        /// It does not set the label
        /// </summary>
        public const string None = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="major">The major number</param>
        /// <param name="minor">The minor number</param>
        /// <param name="patch">The path number</param>
        public SemanticVersion(int major, int minor, int patch)
        {
            // Prevent negative numbers
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major), "Major version number cannot be negative.");
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor), "Minor version number cannot be negative.");
            if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch), "Patch version number cannot be negative.");

            // Set properties
            mMajor = major;
            mMinor = minor;
            mPatch = patch;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="major">The major number</param>
        /// <param name="minor">The minor number</param>
        /// <param name="patch">The path number</param>
        /// <param name="label">The version type label</param>
        public SemanticVersion(int major, int minor, int patch, string label)
        {
            // Prevent negative numbers
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major), "Major version number cannot be negative.");
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor), "Minor version number cannot be negative.");
            if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch), "Patch version number cannot be negative.");
            if (label.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(label), "Label cannot be null or empty.");

            // Set properties
            mMajor = major;
            mMinor = minor;
            mPatch = patch;
            mLabel = label;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="major">The major number</param>
        /// <param name="minor">The minor number</param>
        public SemanticVersion(int major, int minor)
        {
            // Prevent negative numbers
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major), "Major version number cannot be negative.");
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor), "Minor version number cannot be negative.");

            // Set properties
            mMajor = major;
            mMinor = minor;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="major">The major number</param>
        /// <param name="minor">The minor number</param>
        /// <param name="label">The version type label</param>
        public SemanticVersion(int major, int minor, string label)
        {
            // Prevent negative numbers
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major), "Major version number cannot be negative.");
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor), "Minor version number cannot be negative.");
            if (label.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(label), "Label cannot be null or empty.");

            // Set properties
            mMajor = major;
            mMinor = minor;
            mLabel = label;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="version">The string version that will be parsed</param>
        public SemanticVersion(string version)
        {
            var sv = Parse(version);
            mMajor = sv.Major;
            mMinor = sv.Minor;
            mPatch = sv.Patch;
            mLabel = sv.Label;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="version">A <see cref="Version"/> to make it semantic</param>
        public SemanticVersion(Version version)
        {
            // Prevent bugs
            if (version == null) throw new ArgumentNullException(nameof(version));

            // Set properties
            mMajor = version.Major;
            mMinor = version.Minor;
            mPatch = version.Build;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="version">A <see cref="Version"/> to make it semantic</param>
        /// <param name="label">The version type label</param>
        public SemanticVersion(Version version, string label)
        {
            // Prevent bugs
            if (version == null) throw new ArgumentNullException(nameof(version));
            if (label.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(label), "Label cannot be null or empty.");

            // Set properties
            mMajor = version.Major;
            mMinor = version.Minor;
            mPatch = version.Build;
            mLabel = label;
        }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/>
        /// </summary>
        public SemanticVersion()
        {
            mMajor = 0;
            mMinor = 0;
            mPatch = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The major version number
        /// </summary>
        public int Major => mMajor;

        /// <summary>
        /// The minor version number
        /// </summary>
        public int Minor => mMinor;

        /// <summary>
        /// The patch version number
        /// </summary>
        public int Patch => mPatch;

        /// <summary>
        /// The version type label
        /// </summary>
        public string Label => mLabel;

        #endregion

        #region Methods

        /// <summary>
        /// Compares two <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="other">The <see cref="SemanticVersion"/> to compare</param>
        /// <param name="compareLabel">Indicates if the label must be compared.</param>
        /// <returns>Returns a <see cref="VersionCompare"/> that defines the result of the comparison</returns>
        public VersionCompare CompareTo(SemanticVersion other, bool compareLabel = false)
        {
            // Return 1 if the comparison was failed
            if (ReferenceEquals(other, null))
                return VersionCompare.Greater;

            // Return source version greater
            if (other == null)
                return VersionCompare.Greater;

            if (Major != other.Major)
                if (Major > other.Major)
                    return VersionCompare.Greater;
                else
                    return VersionCompare.Less;

            if (Minor != other.Minor)
                if (Minor > other.Minor)
                    return VersionCompare.Greater;
                else
                    return VersionCompare.Less;

            if (Patch != other.Patch)
                if (Patch > other.Patch)
                    return VersionCompare.Greater;
                else
                    return VersionCompare.Less;

            if(compareLabel)
                return CompareDefaultLabels(Label, other.Label);

            return VersionCompare.Equals;
        }

        /// <summary>
        /// Sets the label for the semantic version
        /// </summary>
        /// <param name="label">The label to set</param>
        public void SetLabel(string label) => mLabel = label;

        /// <summary>
        /// Increments 1 to the major version
        /// </summary>
        public SemanticVersion IncrementMajor(bool removeLabel = false)
        {
            mMajor++;

            if (removeLabel)
                mLabel = None;

            return this;
        }

        /// <summary>
        /// Increments 1 to the minor version
        /// </summary>
        public SemanticVersion IncrementMinor(bool removeLabel = false)
        {
            mMinor++;

            if (removeLabel)
                mLabel = None;

            return this;
        }

        /// <summary>
        /// Increments 1 to the minor version
        /// </summary>
        public SemanticVersion IncrementPatch(bool removeLabel = false)
        {
            mPatch++;

            if (removeLabel)
                mLabel = None;

            return this;
        }

        /// <summary>
        /// Compare to another <see cref="SemanticVersion"/>
        /// </summary>
        public bool Equals(SemanticVersion other)
        {
            if (other == null)
                return false;

            if ((Major != other.Major) ||
                (Minor != other.Minor) ||
                (Patch != other.Patch) ||
                (Label != other.Label))
                return false;

            return true;
        }

        /// <summary>
        /// Compare to another <see cref="SemanticVersion"/>
        /// </summary>
        public override bool Equals(object obj)
        {
            SemanticVersion v = obj as SemanticVersion;
            if (v == null)
                return false;

            return Equals(v);
        }

        /// <summary>
        /// Get the hash code
        /// </summary>
        public override int GetHashCode()
        {
            int accumulator = 0;

            accumulator |= (Major & 0x0000000F) << 28;
            accumulator |= (Minor & 0x000000FF) << 20;
            accumulator |= (Patch & 0x000000FF) << 12;
            accumulator |= (Label.GetHashCode());

            return accumulator;
        }

        /// <summary>
        /// Returns an string representation of the <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="showLabel">Indicates if the label must be shown</param>
        public string ToString(bool showLabel = true)
        {
            string returnStr = $"{Major}{SeparatorChars[0]}{Minor}{SeparatorChars[0]}{Patch}";

            if (!Label.IsNullOrWhitespace() && showLabel)
                returnStr += $"-{Label}";

            return returnStr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => ToString(false);

        #endregion

        #region Static Methods

        /// <summary>
        /// Tries to convert the string representation of a version number to an equivalent <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="input">The string input to parse</param>
        /// <param name="result">The result of the parse</param>
        public static bool TryParse(string input, out SemanticVersion result)
        {
            // Prevent bugs
            if (input.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(input));

            // Define variables
            int major, minor, patch = 0;
            result = new SemanticVersion();
            string label = null;

            // Get components
            string[] components = input.Split(SeparatorChars);
            int componentsLength = components.Length;

            // The string has a incorrect format
            if (componentsLength < 2 || componentsLength > 4)
                throw new ArgumentException("The input string has an incorrect format.", nameof(input));

            if (!TryParseComponent(components[0], "major", out major))
                return false;

            if (!TryParseComponent(components[1], "minor", out minor))
                return false;

            componentsLength -= 2;

            // If there are more components
            if(componentsLength > 0)
            {
                // Try to parse the component to the patch, if failed, it means its the label
                if(!TryParseComponent(components[2], "patch", out patch))
                    label = components[2]; // Set label

                // If the code reachs this section, it means the patch was parsed
                componentsLength--;

                // And here, the label must be parsed
                if(componentsLength > 0)
                    label = components[3];
            }

            // Create the semantic version
            if(label == null)
                result = new SemanticVersion(major, minor, patch);
            else
                result = new SemanticVersion(major, minor, patch, label);

            return true;
        }

        /// <summary>
        /// Tries to convert the string representation of a version number to an equivalent <see cref="SemanticVersion"/>
        /// </summary>
        /// <param name="input">The string input to parse</param>
        /// <returns></returns>
        public static SemanticVersion Parse(string input)
        {
            // Prevent bugs
            if (input.IsNullOrWhitespace()) throw new ArgumentNullException(nameof(input));

            // Try to parse
            if (!TryParse(input, out SemanticVersion semanticVersion))
                throw new InvalidOperationException("The string input could not be parsed into a SemanticVersion."); // Throw exception if could not

            // Return the version if success
            return semanticVersion;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Compare if two <see cref="SemanticVersion"/> are equals
        /// </summary>
        public static bool operator ==(SemanticVersion version1, SemanticVersion version2)
        {
            if (ReferenceEquals(version1, null))
                return ReferenceEquals(version2, null);

            return version1.Equals(version2);
        }

        /// <summary>
        /// Compare if two <see cref="SemanticVersion"/> are not equals
        /// </summary>
        public static bool operator !=(SemanticVersion version1, SemanticVersion version2)
            => !(version1 == version2);

        /// <summary>
        /// Compare if one <see cref="SemanticVersion"/> is less than another
        /// </summary>
        public static bool operator <(SemanticVersion version1, SemanticVersion version2)
        {
            if (version1 == null) throw new ArgumentNullException("version1");

            return version1.CompareTo(version2) == VersionCompare.Less;
        }

        /// <summary>
        /// Compare if one <see cref="SemanticVersion"/> is less or equals than another
        /// </summary>
        public static bool operator <=(SemanticVersion version1, SemanticVersion version2)
            => (version1 == version2) || (version1 < version2);

        /// <summary>
        /// Compare if one <see cref="SemanticVersion"/> is greater than another
        /// </summary>
        public static bool operator >(SemanticVersion version1, SemanticVersion version2)
        {
            if (version1 == null) throw new ArgumentNullException("version1");

            return version2 < version1;
        }

        /// <summary>
        /// Compare if one <see cref="SemanticVersion"/> is greater or equals than another
        /// </summary>
        public static bool operator >=(SemanticVersion version1, SemanticVersion version2)
            => (version1 == version2) || (version1 > version2);

        /// <summary>
        /// Implicit operator to convert an string to a <see cref="SemanticVersion"/>
        /// </summary>
        public static implicit operator SemanticVersion(string version)
            => new SemanticVersion(version);

        #endregion

        #region Private Helper Methods

        private static bool TryParseComponent(string component, string componentName, out int parsedComponent)
        {
            bool result = int.TryParse(component, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedComponent);

            if (!result)
                return false;

            if (parsedComponent < 0)
                throw new ArgumentOutOfRangeException(componentName);

            return true;
        }

        private static VersionCompare CompareDefaultLabels(string label1, string label2)
        {
            // Check if there are labels
            if (label1 == None && label2 == None) return VersionCompare.Equals;
            else if (label1 == None && label2 != None) return VersionCompare.Less;
            else if (label1 != None && label2 == None) return VersionCompare.Greater;

            // Check if they are compile time labels
            if(label1 != Alpha && label1 != Beta && label1 != PreRelease && label1 != Release) throw new InvalidOperationException($"Cannot compare the label '{label1}'");
            if(label2 != Alpha && label2 != Beta && label2 != PreRelease && label2 != Release) throw new InvalidOperationException($"Cannot compare the label '{label2}'.");

            // Check
            if (label1 == Alpha)
            {
                if (label2 == Alpha) return VersionCompare.Equals;
                else return VersionCompare.Less;
            }
            else if (label1 == Beta)
            {
                if (label2 == Alpha) return VersionCompare.Greater;
                else if (label2 == Beta) return VersionCompare.Equals;
                else return VersionCompare.Less;
            }
            else if (label1 == PreRelease)
            {
                if (label2 == Alpha || label2 == Beta) return VersionCompare.Greater;
                else if (label2 == PreRelease) return VersionCompare.Equals;
                else return VersionCompare.Less;
            }
            else if (label1 == Release)
            {
                if (label2 == Release) return VersionCompare.Equals;
                else return VersionCompare.Greater;
            }

            return VersionCompare.Equals;
        }

        #endregion
    }

    /// <summary>
    /// The possible values for a <see cref="SemanticVersion"/> comparison
    /// </summary>
    public enum VersionCompare
    {
        /// <summary>
        /// The two versions are equals
        /// </summary>
        Equals = 0,

        /// <summary>
        /// Source version is greater than compared version
        /// </summary>
        Greater = 1,

        /// <summary>
        /// Source version is less than compared version
        /// </summary>
        Less = -1
    }
}
