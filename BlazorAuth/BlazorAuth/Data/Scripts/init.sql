-- ==============================================
-- Roles Table
-- ==============================================
CREATE TABLE "AspNetRoles" (
                               "Id" TEXT PRIMARY KEY,
                               "Name" VARCHAR(256),
                               "NormalizedName" VARCHAR(256) UNIQUE,
                               "ConcurrencyStamp" TEXT
);

-- ==============================================
-- Users Table
-- ==============================================
CREATE TABLE "AspNetUsers" (
                               "Id" TEXT PRIMARY KEY,
                               "UserName" VARCHAR(256),
                               "NormalizedUserName" VARCHAR(256) UNIQUE,
                               "Email" VARCHAR(256),
                               "NormalizedEmail" VARCHAR(256),
                               "EmailConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
                               "PasswordHash" TEXT,
                               "SecurityStamp" TEXT,
                               "ConcurrencyStamp" TEXT,
                               "PhoneNumber" VARCHAR(256),
                               "PhoneNumberConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
                               "TwoFactorEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
                               "LockoutEnd" TIMESTAMPTZ,
                               "LockoutEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
                               "AccessFailedCount" INTEGER NOT NULL DEFAULT 0
);

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
CREATE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

-- ==============================================
-- Role Claims
-- ==============================================
CREATE TABLE "AspNetRoleClaims" (
                                    "Id" SERIAL PRIMARY KEY,
                                    "RoleId" TEXT NOT NULL REFERENCES "AspNetRoles"("Id") ON DELETE CASCADE,
                                    "ClaimType" TEXT,
                                    "ClaimValue" TEXT
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

-- ==============================================
-- User Claims
-- ==============================================
CREATE TABLE "AspNetUserClaims" (
                                    "Id" SERIAL PRIMARY KEY,
                                    "UserId" TEXT NOT NULL REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE,
                                    "ClaimType" TEXT,
                                    "ClaimValue" TEXT
);

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

-- ==============================================
-- User Logins
-- ==============================================
CREATE TABLE "AspNetUserLogins" (
                                    "LoginProvider" VARCHAR(128) NOT NULL,
                                    "ProviderKey" VARCHAR(128) NOT NULL,
                                    "ProviderDisplayName" TEXT,
                                    "UserId" TEXT NOT NULL REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE,
                                    PRIMARY KEY("LoginProvider", "ProviderKey")
);

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

-- ==============================================
-- User Roles
-- ==============================================
CREATE TABLE "AspNetUserRoles" (
                                   "UserId" TEXT NOT NULL REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE,
                                   "RoleId" TEXT NOT NULL REFERENCES "AspNetRoles"("Id") ON DELETE CASCADE,
                                   PRIMARY KEY("UserId", "RoleId")
);

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

-- ==============================================
-- User Tokens
-- ==============================================
CREATE TABLE "AspNetUserTokens" (
                                    "UserId" TEXT NOT NULL REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE,
                                    "LoginProvider" VARCHAR(128) NOT NULL,
                                    "Name" VARCHAR(128) NOT NULL,
                                    "Value" TEXT,
                                    PRIMARY KEY("UserId", "LoginProvider", "Name")
);

-- ==============================================
-- User Passkeys
-- ==============================================
CREATE TABLE "AspNetUserPasskeys" (
                                      "CredentialId" BYTEA PRIMARY KEY,
                                      "UserId" TEXT NOT NULL REFERENCES "AspNetUsers"("Id") ON DELETE CASCADE,
                                      "Data" JSONB NOT NULL
);

CREATE INDEX "IX_AspNetUserPasskeys_UserId" ON "AspNetUserPasskeys" ("UserId");
