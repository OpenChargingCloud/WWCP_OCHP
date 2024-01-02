/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OCHP <https://github.com/OpenChargingCloud/WWCP_OCHP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The unique identification of an OCHP live authentication.
    /// </summary>
    public class LiveAuth_Id : IId,
                               IEquatable <LiveAuth_Id>,
                               IComparable<LiveAuth_Id>

    {

        #region Data

        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the tag identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new live authentication identification
        /// based on the given string.
        /// </summary>
        private LiveAuth_Id(String  Id)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id),  "The identification must not be null!");

            #endregion

            this.InternalId  = Id;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a live authentication identification.
        /// </summary>
        public static LiveAuth_Id Parse(String Text)

            => new LiveAuth_Id(Text);

        #endregion

        #region TryParse(Text, out LiveAuthId)

        /// <summary>
        /// Parse the given string as a live authentication identification.
        /// </summary>
        public static Boolean TryParse(String Text, out LiveAuth_Id LiveAuthId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                LiveAuthId = null;
                return false;
            }

            #endregion

            try
            {

                LiveAuthId = new LiveAuth_Id(Text);

                return true;

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            LiveAuthId = null;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this live authentication identification.
        /// </summary>
        public LiveAuth_Id Clone

            => new LiveAuth_Id(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (LiveAuthId1, LiveAuthId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LiveAuthId1">A live authentication identification.</param>
        /// <param name="LiveAuthId2">Another live authentication identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (LiveAuth_Id LiveAuthId1, LiveAuth_Id LiveAuthId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(LiveAuthId1, LiveAuthId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) LiveAuthId1 == null) || ((Object) LiveAuthId2 == null))
                return false;

            if ((Object) LiveAuthId1 == null)
                throw new ArgumentNullException(nameof(LiveAuthId1),  "The given live authentication identification must not be null!");

            return LiveAuthId1.Equals(LiveAuthId2);

        }

        #endregion

        #region Operator != (LiveAuthId1, LiveAuthId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LiveAuthId1">A live authentication identification.</param>
        /// <param name="LiveAuthId2">Another live authentication identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (LiveAuth_Id LiveAuthId1, LiveAuth_Id LiveAuthId2)
            => !(LiveAuthId1 == LiveAuthId2);

        #endregion

        #region Operator <  (LiveAuthId1, LiveAuthId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LiveAuthId1">A live authentication identification.</param>
        /// <param name="LiveAuthId2">Another live authentication identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (LiveAuth_Id LiveAuthId1, LiveAuth_Id LiveAuthId2)
        {

            if ((Object) LiveAuthId1 == null)
                throw new ArgumentNullException(nameof(LiveAuthId1),  "The given live authentication identification must not be null!");

            return LiveAuthId1.CompareTo(LiveAuthId2) < 0;

        }

        #endregion

        #region Operator <= (LiveAuthId1, LiveAuthId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LiveAuthId1">A live authentication identification.</param>
        /// <param name="LiveAuthId2">Another live authentication identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (LiveAuth_Id LiveAuthId1, LiveAuth_Id LiveAuthId2)
            => !(LiveAuthId1 > LiveAuthId2);

        #endregion

        #region Operator >  (LiveAuthId1, LiveAuthId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LiveAuthId1">A live authentication identification.</param>
        /// <param name="LiveAuthId2">Another live authentication identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (LiveAuth_Id LiveAuthId1, LiveAuth_Id LiveAuthId2)
        {

            if ((Object) LiveAuthId1 == null)
                throw new ArgumentNullException(nameof(LiveAuthId1),  "The given live authentication identification must not be null!");

            return LiveAuthId1.CompareTo(LiveAuthId2) > 0;

        }

        #endregion

        #region Operator >= (LiveAuthId1, LiveAuthId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LiveAuthId1">A live authentication identification.</param>
        /// <param name="LiveAuthId2">Another live authentication identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (LiveAuth_Id LiveAuthId1, LiveAuth_Id LiveAuthId2)
            => !(LiveAuthId1 < LiveAuthId2);

        #endregion

        #endregion

        #region IComparable<LiveAuth_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a live authentication identification.
            var LiveAuthId = Object as LiveAuth_Id;
            if ((Object) LiveAuthId == null)
                throw new ArgumentException("The given object is not a LiveAuthId!", nameof(Object));

            return CompareTo(LiveAuthId);

        }

        #endregion

        #region CompareTo(LiveAuthId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="LiveAuthId">An object to compare with.</param>
        public Int32 CompareTo(LiveAuth_Id LiveAuthId)
        {

            if ((Object) LiveAuthId == null)
                throw new ArgumentNullException(nameof(LiveAuthId),  "The given live authentication identification must not be null!");

            return String.Compare(InternalId, LiveAuthId.InternalId, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<LiveAuth_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a live authentication identification.
            var LiveAuthId = Object as LiveAuth_Id;
            if ((Object) LiveAuthId == null)
                return false;

            return this.Equals(LiveAuthId);

        }

        #endregion

        #region Equals(LiveAuthId)

        /// <summary>
        /// Compares two live authentication identifications for equality.
        /// </summary>
        /// <param name="LiveAuthId">A live authentication identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(LiveAuth_Id LiveAuthId)
        {

            if ((Object) LiveAuthId == null)
                return false;

            return InternalId.Equals(LiveAuthId.InternalId);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
