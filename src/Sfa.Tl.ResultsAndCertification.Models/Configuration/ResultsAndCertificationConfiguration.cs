﻿namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class ResultsAndCertificationConfiguration
    {
        /// <summary>
        /// Gets or sets the redis settings.
        /// </summary>
        /// <value>
        /// The redis settings.
        /// </value>
        public RedisSettings RedisSettings { get; set; }

        /// <summary>
        /// Gets or sets the BLOB storage settings.
        /// </summary>
        /// <value>
        /// The BLOB storage settings.
        /// </value>
        public BlobStorageSettings BlobStorageSettings { get; set; }

        /// <summary>
        /// Gets or sets the data protection settings.
        /// </summary>
        /// <value>
        /// The data protection settings.
        /// </value>
        public DataProtectionSettings DataProtectionSettings { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        public string SqlConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the SQL connection string for Integration Tests.
        /// </summary>
        /// <value>
        /// The SQL connection string for Integration Tests.
        /// </value>
        public string IntTestSqlConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the BLOB storage connection string.
        /// </summary>
        /// <value>
        /// The BLOB storage connection string.
        /// </value>
        public string BlobStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the gov uk notify API key.
        /// </summary>
        /// <value>
        /// The gov uk notify API key.
        /// </value>
        public string GovUkNotifyApiKey { get; set; }

        /// <summary>
        /// Gets or sets the tlevel queried support email address.
        /// </summary>
        /// <value>
        /// The tlevel queried support email address.
        /// </value>
        public string TlevelQueriedSupportEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the technical support email address.
        /// </summary>
        /// <value>
        /// The technical support email address.
        /// </value>
        public string TechnicalSupportEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the technical feedback email address.
        /// </summary>
        /// <value>
        /// The feedback email address.
        /// </value>
        public string FeedbackEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the dfe sign in settings.
        /// </summary>
        /// <value>
        /// The dfe sign in settings.
        /// </value>
        public DfeSignInSettings DfeSignInSettings { get; set; }

        /// <summary>
        /// Gets or sets the results and certification Internal API settings.
        /// </summary>
        /// <value>
        /// The results and certification Internal API settings.
        /// </value>
        public ResultsAndCertificationInternalApiSettings ResultsAndCertificationInternalApiSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dev.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dev; otherwise, <c>false</c>.
        /// </value>
        public bool IsDevevelopment { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [bypass dfe sign in].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bypass dfe sign in]; otherwise, <c>false</c>.
        /// </value>
        public bool BypassDfeSignIn { get; set; }

        /// <summary>
        /// Gets or sets the learning record service settings.
        /// </summary>
        /// <value>
        /// The learning record service settings.
        /// </value>
        public LearningRecordServiceSettings LearningRecordServiceSettings { get; set; }

        public string KeyVaultUri { get; set; }
    }
}
