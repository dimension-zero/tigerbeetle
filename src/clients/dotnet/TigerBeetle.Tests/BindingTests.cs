using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace TigerBeetle.Tests;

[TestClass]
public class BindingTests
{
    [TestMethod]
    public void Accounts()
    {
        var account = new Account();
        account.Id = 100;
        Assert.AreEqual(account.Id, (UInt128)100);

        account.UserData128 = 101;
        Assert.AreEqual(account.UserData128, (UInt128)101);

        account.UserData64 = 102;
        Assert.AreEqual(account.UserData64, (ulong)102L);

        account.UserData32 = 103;
        Assert.AreEqual(account.UserData32, (uint)103);

        account.Reserved = 0;
        Assert.AreEqual(account.Reserved, (uint)0);

        account.Ledger = 104;
        Assert.AreEqual(account.Ledger, (uint)104);

        account.Code = 105;
        Assert.AreEqual(account.Code, (ushort)105);

        var flags = AccountFlags.Linked | AccountFlags.DebitsMustNotExceedCredits | AccountFlags.CreditsMustNotExceedDebits;
        account.Flags = flags;
        Assert.AreEqual(account.Flags, flags);

        account.DebitsPending = 1001;
        Assert.AreEqual(account.DebitsPending, (UInt128)1001);

        account.CreditsPending = 1002;
        Assert.AreEqual(account.CreditsPending, (UInt128)1002);

        account.DebitsPosted = 1003;
        Assert.AreEqual(account.DebitsPosted, (UInt128)1003);

        account.CreditsPosted = 1004;
        Assert.AreEqual(account.CreditsPosted, (UInt128)1004);

        account.Timestamp = 99_999;
        Assert.AreEqual(account.Timestamp, (ulong)99_999);
    }

    [TestMethod]
    public void AccountsDefault()
    {
        var account = new Account(); ;
        Assert.AreEqual(account.Id, UInt128.Zero);
        Assert.AreEqual(account.UserData128, UInt128.Zero);
        Assert.AreEqual(account.UserData64, (ulong)0);
        Assert.AreEqual(account.UserData32, (uint)0);
        Assert.AreEqual(account.Reserved, (uint)0);
        Assert.AreEqual(account.Ledger, (uint)0);
        Assert.AreEqual(account.Code, (ushort)0);
        Assert.AreEqual(account.Flags, AccountFlags.None);
        Assert.AreEqual(account.DebitsPending, (UInt128)0);
        Assert.AreEqual(account.CreditsPending, (UInt128)0);
        Assert.AreEqual(account.DebitsPosted, (UInt128)0);
        Assert.AreEqual(account.CreditsPosted, (UInt128)0);
        Assert.AreEqual(account.Timestamp, (UInt128)0);
    }

    [TestMethod]
    public void AccountsSerialize()
    {
        var expected = new byte[Account.SIZE];
        using (var writer = new BinaryWriter(new MemoryStream(expected)))
        {
            writer.Write(10L); // Id (lsb)
            writer.Write(11L); // Id (msb)
            writer.Write(100L); // DebitsPending (lsb)
            writer.Write(110L); // DebitsPending (msb)
            writer.Write(200L); // DebitsPosted (lsb)
            writer.Write(210L); // DebitsPosted (msb)
            writer.Write(300L); // CreditPending (lsb)
            writer.Write(310L); // CreditPending (msb)
            writer.Write(400L); // CreditsPosted (lsb)
            writer.Write(410L); // CreditsPosted (msb)
            writer.Write(1000L); // UserData128 (lsb)
            writer.Write(1100L); // UserData128 (msb)
            writer.Write(2000L); // UserData64
            writer.Write(3000); // UserData32
            writer.Write(0); // Reserved
            writer.Write(720); // Ledger
            writer.Write((short)1); // Code
            writer.Write((short)1); // Flags
            writer.Write(999L); // Timestamp
        }

        var account = new Account
        {
            Id = new UInt128(11L, 10L),
            DebitsPending = new UInt128(110L, 100L),
            DebitsPosted = new UInt128(210L, 200L),
            CreditsPending = new UInt128(310L, 300L),
            CreditsPosted = new UInt128(410L, 400L),
            UserData128 = new UInt128(1100L, 1000L),
            UserData64 = 2000L,
            UserData32 = 3000,
            Ledger = 720,
            Code = 1,
            Flags = AccountFlags.Linked,
            Timestamp = 999,
        };

        var serialized = MemoryMarshal.AsBytes<Account>(new Account[] { account }).ToArray();
        Assert.IsTrue(expected.SequenceEqual(serialized));
    }

    [TestMethod]
    public void CreateAccountResults()
    {
        var result = new CreateAccountResult();

        result.Timestamp = 1;
        Assert.AreEqual(result.Timestamp, (uint)1);

        result.Status = CreateAccountStatus.Exists;
        Assert.AreEqual(result.Status, CreateAccountStatus.Exists);
    }

    [TestMethod]
    public void CreateAccountResultsSerialize()
    {
        var expected = new byte[CreateAccountResult.SIZE];
        using (var writer = new BinaryWriter(new MemoryStream(expected)))
        {
            writer.Write(99_999L); // Timestamp
            writer.Write((uint)CreateAccountStatus.IdMustNotBeIntMax); // Result
            writer.Write((uint)1); // Reserved
        }

        var result = new CreateAccountResult
        {
            Timestamp = 99_999,
            Status = CreateAccountStatus.IdMustNotBeIntMax,
            Reserved = 1,
        };

        var serialized = MemoryMarshal.AsBytes<CreateAccountResult>(new CreateAccountResult[] { result }).ToArray();
        Assert.IsTrue(expected.SequenceEqual(serialized));
    }

    [TestMethod]
    public void Transfers()
    {
        var transfer = new Transfer();

        transfer.Id = 100;
        Assert.AreEqual(transfer.Id, (UInt128)100);

        transfer.DebitAccountId = 101;
        Assert.AreEqual(transfer.DebitAccountId, (UInt128)101);

        transfer.CreditAccountId = 102;
        Assert.AreEqual(transfer.CreditAccountId, (UInt128)102);

        transfer.Amount = 1001;
        Assert.AreEqual(transfer.Amount, (UInt128)1001);

        transfer.PendingId = 103;
        Assert.AreEqual(transfer.PendingId, (UInt128)103);

        transfer.UserData128 = 104;
        Assert.AreEqual(transfer.UserData128, (UInt128)104);

        transfer.UserData64 = 105;
        Assert.AreEqual(transfer.UserData64, (ulong)105);

        transfer.UserData32 = 106;
        Assert.AreEqual(transfer.UserData32, (uint)106);

        transfer.Timeout = 107;
        Assert.AreEqual(transfer.Timeout, (uint)107);

        transfer.Ledger = 108;
        Assert.AreEqual(transfer.Ledger, (uint)108);

        transfer.Code = 109;
        Assert.AreEqual(transfer.Code, (ushort)109);

        var flags = TransferFlags.Linked | TransferFlags.PostPendingTransfer | TransferFlags.VoidPendingTransfer;
        transfer.Flags = flags;
        Assert.AreEqual(transfer.Flags, flags);

        transfer.Timestamp = 99_999;
        Assert.AreEqual(transfer.Timestamp, (ulong)99_999);
    }

    [TestMethod]
    public void TransferDefault()
    {
        var transfer = new Transfer();
        Assert.AreEqual(transfer.Id, (UInt128)0);
        Assert.AreEqual(transfer.DebitAccountId, (UInt128)0);
        Assert.AreEqual(transfer.CreditAccountId, (UInt128)0);
        Assert.AreEqual(transfer.Amount, (UInt128)0);
        Assert.AreEqual(transfer.PendingId, (UInt128)0);
        Assert.AreEqual(transfer.UserData128, (UInt128)0);
        Assert.AreEqual(transfer.UserData64, (ulong)0);
        Assert.AreEqual(transfer.UserData32, (uint)0);
        Assert.AreEqual(transfer.Timeout, (uint)0);
        Assert.AreEqual(transfer.Ledger, (uint)0);
        Assert.AreEqual(transfer.Code, (ushort)0);
        Assert.AreEqual(transfer.Flags, TransferFlags.None);
        Assert.AreEqual(transfer.Timestamp, (ulong)0);
    }

    [TestMethod]
    public void TransfersSerialize()
    {
        var expected = new byte[Transfer.SIZE];
        using (var writer = new BinaryWriter(new MemoryStream(expected)))
        {
            writer.Write(10L); // Id (lsb)
            writer.Write(11L); // Id (msb)
            writer.Write(100L); // DebitAccountId (lsb)
            writer.Write(110L); // DebitAccountId (msb)
            writer.Write(200L); // CreditAccountId (lsb)
            writer.Write(210L); // CreditAccountId (msb)
            writer.Write(300L); // Amount (lsb)
            writer.Write(310L); // Amount (msb)
            writer.Write(400L); // PendingId (lsb)
            writer.Write(410L); // PendingId (msb)
            writer.Write(1000L); // UserData128 (lsb)
            writer.Write(1100L); // UserData128 (msb)
            writer.Write(2000L); // UserData64
            writer.Write(3000); // UserData32
            writer.Write(999); // Timeout
            writer.Write(720); // Ledger
            writer.Write((short)1); // Code
            writer.Write((short)1); // Flags
            writer.Write(99_999L); // Timestamp
        }

        var transfer = new Transfer
        {
            Id = new UInt128(11L, 10L),
            DebitAccountId = new UInt128(110L, 100L),
            CreditAccountId = new UInt128(210L, 200L),
            Amount = new UInt128(310L, 300L),
            PendingId = new UInt128(410L, 400L),
            UserData128 = new UInt128(1100L, 1000L),
            UserData64 = 2000L,
            UserData32 = 3000,
            Timeout = 999,
            Ledger = 720,
            Code = 1,
            Flags = TransferFlags.Linked,
            Timestamp = 99_999,
        };

        var serialized = MemoryMarshal.AsBytes<Transfer>(new Transfer[] { transfer }).ToArray();
        Assert.IsTrue(expected.SequenceEqual(serialized));
    }

    [TestMethod]
    public void CreateTransferResults()
    {
        var result = new CreateTransferResult();

        result.Timestamp = 1;
        Assert.AreEqual(result.Timestamp, (uint)1);

        result.Status = CreateTransferStatus.Exists;
        Assert.AreEqual(result.Status, CreateTransferStatus.Exists);
    }

    [TestMethod]
    public void CreateTransferResultsSerialize()
    {
        var expected = new byte[CreateTransferResult.SIZE];
        using (var writer = new BinaryWriter(new MemoryStream(expected)))
        {
            writer.Write(99_999L); // Timestamp
            writer.Write((uint)CreateTransferStatus.IdMustNotBeIntMax); // Result
            writer.Write((uint)1); // Reserved
        }

        var result = new CreateTransferResult
        {
            Timestamp = 99_999,
            Status = CreateTransferStatus.IdMustNotBeIntMax,
            Reserved = 1,
        };

        var serialized = MemoryMarshal.AsBytes<CreateTransferResult>(new CreateTransferResult[] { result }).ToArray();
        Assert.IsTrue(expected.SequenceEqual(serialized));
    }

    [TestMethod]
    public void ChangeEvents()
    {
        var changeEvent = new ChangeEvent();

        changeEvent.TransferId = 100;
        Assert.AreEqual(changeEvent.TransferId, (UInt128)100);

        changeEvent.TransferAmount = 101;
        Assert.AreEqual(changeEvent.TransferAmount, (UInt128)101);

        changeEvent.TransferPendingId = 102;
        Assert.AreEqual(changeEvent.TransferPendingId, (UInt128)102);

        changeEvent.TransferUserData128 = 103;
        Assert.AreEqual(changeEvent.TransferUserData128, (UInt128)103);

        changeEvent.TransferUserData64 = 104;
        Assert.AreEqual(changeEvent.TransferUserData64, (ulong)104);

        changeEvent.TransferUserData32 = 105;
        Assert.AreEqual(changeEvent.TransferUserData32, (uint)105);

        changeEvent.TransferTimeout = 106;
        Assert.AreEqual(changeEvent.TransferTimeout, (uint)106);

        changeEvent.TransferCode = 107;
        Assert.AreEqual(changeEvent.TransferCode, (ushort)107);

        var transferFlags = TransferFlags.Linked | TransferFlags.PostPendingTransfer;
        changeEvent.TransferFlags = transferFlags;
        Assert.AreEqual(changeEvent.TransferFlags, transferFlags);

        changeEvent.Ledger = 108;
        Assert.AreEqual(changeEvent.Ledger, (uint)108);

        changeEvent.Type = ChangeEventType.TwoPhasePosted;
        Assert.AreEqual(changeEvent.Type, ChangeEventType.TwoPhasePosted);

        changeEvent.DebitAccountId = 200;
        Assert.AreEqual(changeEvent.DebitAccountId, (UInt128)200);

        changeEvent.DebitAccountDebitsPending = 201;
        Assert.AreEqual(changeEvent.DebitAccountDebitsPending, (UInt128)201);

        changeEvent.DebitAccountDebitsPosted = 202;
        Assert.AreEqual(changeEvent.DebitAccountDebitsPosted, (UInt128)202);

        changeEvent.DebitAccountCreditsPending = 203;
        Assert.AreEqual(changeEvent.DebitAccountCreditsPending, (UInt128)203);

        changeEvent.DebitAccountCreditsPosted = 204;
        Assert.AreEqual(changeEvent.DebitAccountCreditsPosted, (UInt128)204);

        changeEvent.DebitAccountUserData128 = 205;
        Assert.AreEqual(changeEvent.DebitAccountUserData128, (UInt128)205);

        changeEvent.DebitAccountUserData64 = 206;
        Assert.AreEqual(changeEvent.DebitAccountUserData64, (ulong)206);

        changeEvent.DebitAccountUserData32 = 207;
        Assert.AreEqual(changeEvent.DebitAccountUserData32, (uint)207);

        changeEvent.DebitAccountCode = 208;
        Assert.AreEqual(changeEvent.DebitAccountCode, (ushort)208);

        var debitAccountFlags = AccountFlags.Linked | AccountFlags.History;
        changeEvent.DebitAccountFlags = debitAccountFlags;
        Assert.AreEqual(changeEvent.DebitAccountFlags, debitAccountFlags);

        changeEvent.CreditAccountId = 300;
        Assert.AreEqual(changeEvent.CreditAccountId, (UInt128)300);

        changeEvent.CreditAccountDebitsPending = 301;
        Assert.AreEqual(changeEvent.CreditAccountDebitsPending, (UInt128)301);

        changeEvent.CreditAccountDebitsPosted = 302;
        Assert.AreEqual(changeEvent.CreditAccountDebitsPosted, (UInt128)302);

        changeEvent.CreditAccountCreditsPending = 303;
        Assert.AreEqual(changeEvent.CreditAccountCreditsPending, (UInt128)303);

        changeEvent.CreditAccountCreditsPosted = 304;
        Assert.AreEqual(changeEvent.CreditAccountCreditsPosted, (UInt128)304);

        changeEvent.CreditAccountUserData128 = 305;
        Assert.AreEqual(changeEvent.CreditAccountUserData128, (UInt128)305);

        changeEvent.CreditAccountUserData64 = 306;
        Assert.AreEqual(changeEvent.CreditAccountUserData64, (ulong)306);

        changeEvent.CreditAccountUserData32 = 307;
        Assert.AreEqual(changeEvent.CreditAccountUserData32, (uint)307);

        changeEvent.CreditAccountCode = 308;
        Assert.AreEqual(changeEvent.CreditAccountCode, (ushort)308);

        var creditAccountFlags = AccountFlags.DebitsMustNotExceedCredits;
        changeEvent.CreditAccountFlags = creditAccountFlags;
        Assert.AreEqual(changeEvent.CreditAccountFlags, creditAccountFlags);

        changeEvent.Timestamp = 99_999;
        Assert.AreEqual(changeEvent.Timestamp, (ulong)99_999);

        changeEvent.TransferTimestamp = 99_998;
        Assert.AreEqual(changeEvent.TransferTimestamp, (ulong)99_998);

        changeEvent.DebitAccountTimestamp = 99_997;
        Assert.AreEqual(changeEvent.DebitAccountTimestamp, (ulong)99_997);

        changeEvent.CreditAccountTimestamp = 99_996;
        Assert.AreEqual(changeEvent.CreditAccountTimestamp, (ulong)99_996);
    }

    [TestMethod]
    public void ChangeEventDefault()
    {
        var changeEvent = new ChangeEvent();
        Assert.AreEqual(changeEvent.TransferId, (UInt128)0);
        Assert.AreEqual(changeEvent.TransferAmount, (UInt128)0);
        Assert.AreEqual(changeEvent.TransferPendingId, (UInt128)0);
        Assert.AreEqual(changeEvent.TransferUserData128, (UInt128)0);
        Assert.AreEqual(changeEvent.TransferUserData64, (ulong)0);
        Assert.AreEqual(changeEvent.TransferUserData32, (uint)0);
        Assert.AreEqual(changeEvent.TransferTimeout, (uint)0);
        Assert.AreEqual(changeEvent.TransferCode, (ushort)0);
        Assert.AreEqual(changeEvent.TransferFlags, TransferFlags.None);
        Assert.AreEqual(changeEvent.Ledger, (uint)0);
        Assert.AreEqual(changeEvent.Type, ChangeEventType.SinglePhase);
        Assert.AreEqual(changeEvent.DebitAccountId, (UInt128)0);
        Assert.AreEqual(changeEvent.DebitAccountFlags, AccountFlags.None);
        Assert.AreEqual(changeEvent.CreditAccountId, (UInt128)0);
        Assert.AreEqual(changeEvent.CreditAccountFlags, AccountFlags.None);
        Assert.AreEqual(changeEvent.Timestamp, (ulong)0);
        Assert.AreEqual(changeEvent.TransferTimestamp, (ulong)0);
        Assert.AreEqual(changeEvent.DebitAccountTimestamp, (ulong)0);
        Assert.AreEqual(changeEvent.CreditAccountTimestamp, (ulong)0);
    }

    [TestMethod]
    public void ChangeEventsFilters()
    {
        var filter = new ChangeEventsFilter();

        filter.TimestampMin = 100;
        Assert.AreEqual(filter.TimestampMin, (ulong)100);

        filter.TimestampMax = 101;
        Assert.AreEqual(filter.TimestampMax, (ulong)101);

        filter.Limit = 102;
        Assert.AreEqual(filter.Limit, (uint)102);
    }

    [TestMethod]
    public void ChangeEventsFilterSerialize()
    {
        var expected = new byte[ChangeEventsFilter.SIZE];
        using (var writer = new BinaryWriter(new MemoryStream(expected)))
        {
            writer.Write(100L); // TimestampMin
            writer.Write(200L); // TimestampMax
            writer.Write(10); // Limit
        }

        var filter = new ChangeEventsFilter
        {
            TimestampMin = 100,
            TimestampMax = 200,
            Limit = 10,
        };

        var serialized = MemoryMarshal.AsBytes<ChangeEventsFilter>(new ChangeEventsFilter[] { filter }).ToArray();
        Assert.IsTrue(expected.SequenceEqual(serialized));
    }
}
