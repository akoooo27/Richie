# Account settings canvas: regression re-test (QA-R)

Independent QA lead, 2026-11-05. I did not build or first-test these boards. This is a report only: no canvas file, spec or published artifact was changed.

**Scope:** all 51 boards of the **Richie Account Settings** canvas (https://claude.ai/artifact/SQiXuTqyASonZHTVzVFW3y, published version `1793825260-e793`), against [`../spec.md`](../spec.md) Revision 2, [`../../how-we-design.md`](../../how-we-design.md) §6, [`../../suiss-ui-guide.md`](../../suiss-ui-guide.md), WCAG 2.2 AA and the identity-only, passwordless product rules.

**Source check:** the Artifact `files` listing of the canvas matches `.design/canvas/project/` byte for byte (53 files: 51 boards, `account.css`, `canvas.json`), so the local sources were tested.

## How this was tested

- **Rig, tuned first** (step 1; documented in `.design/qa-rig/README.md`, "Tuning, 2026-11-05"):
  - Text inside `[inert]` or `aria-hidden="true"` subtrees is skipped by the clipped-text and overlap checks.
  - axe runs with those subtrees excluded. `aria-hidden-focus` runs separately so it still works.
  - A Tab stop drawn by the canvas-only `.is-focused` ring is listed as "drawn focus", not as a missing-focus major.
  - Added `--forced-colors` and `--motion` flags. Every other check is unchanged.
- **Build:** `node .design/qa-rig/build.mjs .design/canvas/project .design/qa-out/regress .design/qa-blobs/foundation` built 51 boards with 0 warnings.
- **Full run:** `run.mjs --out .design/qa-out/regress --port 9600 --variant both --workers 6`.
  - Viewports: 320×640, 375×812, 390×844, 768×1024, 1024×768, 1280×800, 1440×900, 1920×1080, 1280×540 and 844×390, plus 200 % zoom, each in light and dark.
  - The untuned rig was also run on the same build (port 9607) to measure what the tuning removed.
- **Probes** (scripts in `.design/qa-regress/tools/`, ports 9601–9607, each server and browser closed on exit; nothing is left listening on 9600–9610):
  - `probe.mjs`: geometry per state and width, the accessibility tree, the dark-class harness (`.dark` on `<html>`, the §7.1 app mechanism) for 29 state boards at 1440, 768, 390 and 320, forced colours, motion, and real Tab focus with `.is-focused` removed.
  - `axtree.mjs`: Chromium's own accessibility tree (via CDP), for overlay boards. Playwright's `ariaSnapshot` ignores `inert`.
  - `copycheck.py`: every spec §4 string checked against every board.
  - `inject.mjs`: puts copy-only status messages into their slots and measures what moves.
  - `fc-colors.mjs`: the forced-colours system colours.
  - `sheets.mjs`: contact sheets of every board at 1440 and 390.
- **Looked at:** every board at 1440 and 390 (contact sheets). The 11 new or changed boards were also checked at 768 and 320, in light, dark (drawn and harness), at short heights and in forced colours.

**Evidence shorthand:**

| Prefix | Path |
|---|---|
| `RIG/` | `.design/qa-rig/results/regress/` (tuned rig: `report.md`, `report.json`, `shots/`) |
| `UNT/` | `.design/qa-regress/rig-untuned/` (same run, untuned rig) |
| `PRB/` | `.design/qa-regress/probe/` (`probe-aria-geom.json`, `probe-dark-forced-motion-focus.json`, `aria/`, `dark/`, `forced/`, `focus/`) |
| `SH/` | `.design/qa-regress/sheets/` (contact sheets) |
| `CC` | `.design/qa-regress/copycheck.json` |
| `P/` | `.design/canvas/project/` |

## Summary

**Open issues:**

| Severity | Count |
|---|---|
| Blocker | 0 |
| Major | 5 |
| Minor | 18 |

Severity follows the Phase 4 reports:

- **Major:** a WCAG AA failure, a state that moves content against §1.5 rule 8, or board copy or semantics that contradict the spec (a developer can't tell which is right).
- **Minor:** polish, or a spec or board inconsistency with low impact.

Every open issue has an `R-` ID, including the Phase 4 items that are not fixed.

**Phase 4 verification (63 findings):**

| Result | Count | IDs |
|---|---|---|
| Fixed | 47 | 9 of them have a residual logged as a new R finding: A-04, B-02, C-02, C-04, C-05, C-10, C-16, C-17, C-21 |
| Moved to spec | 11 | Revision 2 covers all of them. 3 have a residual board or markup mismatch |
| Not fixed | 5 | A-10, A-14, C-07 (in part), C-22, C-27 |
| Declined (valid or invalid) | 0 | |

**What passes:**

- **Layout holds between states:**
  - The Profile name states and photo-removed keep the Email `h2` in place (692 px at 1440, 1174 px at 390).
  - The passkeys foot no longer pushes "Where you're signed in" at any width.
  - Change-email steps 1 and 2, and step-up B with its error and locked states, are pixel-identical in size at every width from 320×480 to 1440×900.
- **Rig:** no overflow on any fluid board at 320 or at 200 % zoom, no axe violations, and no console errors.
- **Dark:** the dark-class harness on 29 state boards found 0 contrast violations, and the renders are correct.
- **Forced colours:** all four C-20 fixes and the A-16 rule are verified.
- **Reduced motion:** matches §7.6.
- **Workspaces:** all workspace leftovers are gone from `account.css`.
- **Deletion lifecycle:** the new boards match spec §4.7 and §4.8 copy verbatim: dates, 30 + 30 days, sign in to keep, and the signed-out export.

**What fails, in short:**

1. Revision 2's "email code" renames were made in the spec but not on the boards (32 boards).
2. The locked step-up state follows the old Phase 4 suggestion. It focuses the countdown button and uses `role="alert"`, while Revision 2 specifies focus on the error message. The locked code input also has no visible focus.
3. Step-up screens A and B still differ in height.
4. The new Export-request chip breaks the long-email rule.
5. Spec §6 is still stale against `canvas.json` and doesn't list `Profile-photo-removed`.

## Phase 4 verification

| ID | Result | Evidence | Note |
|---|---|---|---|
| A-01 | Fixed | `PRB/probe-aria-geom.json` `pos:*`: action row 36 px in every state. Email `h2` at 692 (1440) and 1174 (390) on Main, name-saved, save-failed and photo-removed | Profile-name-error is 28 px lower because of the field error line. Revision 2 doesn't reserve that line, so this is allowed |
| A-02 | Fixed | `pos:Security*`: sessions `h2` at 712, 896, 908 and 1056 (1440, 768, 390, 320), identical across Security, -waiting and -failed | Status slot on its own row, `account.css:583`, `:2661` |
| A-03 | Fixed | `dlg:Profile-email-*`: title 281 and footer 575 in both steps at 1440. Identical at 1280×540, 844×390 and 320–390 | Ghost panes (`.ra-steps`), `P/Profile-email-change.dc.html:21-47` |
| A-04 | Fixed | `P/Main.dc.html:102`; the sr sentence matches §4.2 | Residual "Hi," against "Hi!": R-21 |
| A-05 | Fixed | `value="Jay"` on name-saved and save-failed. Sidebar "Jay Rivera" (saved) and "Jamie Rivera" (failed) | |
| A-06 | Fixed | `PRB/extreme-sidebar-1440b.png`: "maximiliana.featherstonehaug…@", then the whole domain wrapping at a hyphen | Spec §4.0 and §4.2 now decide two lines |
| A-07 | Fixed | `pos:Security-loading`: 44 px short at 1440, 768 and 390. That is the hidden foot button, as §4.3 specifies | A residual 84 px at 320 comes from real text wrapping more. Accepted |
| A-08 | Fixed | `photo:*:320`: photo row 134 and First name label at 374 on Main, photo-removed and extreme | |
| A-09 | Fixed | `edges:Security:1440`: Change email text ends at 1172, the same as the outline buttons (608 at 640) | |
| A-10 | **Not fixed** | `grid:Main:700`: columns `340px 296px`, messenger 264 | R-07 |
| A-11 | Fixed | `dlg:Security-remove-passkey:1440x900`: 142 and 142 px | |
| A-12 | Fixed | `dlg:Profile-email-code:*`: body `scrollHeight` 184 = `clientHeight` 184 | |
| A-13 | Fixed | `hits:Profile-photo-crop:390`: slider 44 px | |
| A-14 | **Not fixed** | `RIG/shots/Profile-photo-crop/844x390-light.png`: frame still 280 px, Zoom out of view. Scroll cue added | R-06 |
| A-15 | Moved to spec | §7.2 row 25 now states the built metrics (14 14 8, gap 12, `min(250px, 88%)`) | Covered |
| A-16 | Fixed | `PRB/forced/Main-1440x900.png`: the rule under "Back to Richie" is visible | |
| A-17 | Fixed | `grep -i workspace\|ra-count\|ra-msgr-mark account.css` returns nothing. §10.3 drops `buildings` | |
| B-01 | Fixed | `dlg:Stepup-code*:320x640`: title 194 and meta 64 in code, error and locked | The icon is inline and the paragraph balanced |
| B-02 | Fixed (canvas) | The input is `readonly aria-disabled`, not `disabled`. CDP tree: textbox [disabled] keeps focus | The board's focus target conflicts with Revision 2: R-02. The input has no visible focus: R-03 |
| B-03 | Fixed | `P/Data.dc.html:55` matches §4.7 | The lifecycle is now in spec §4.7 and §4.8 |
| B-04 | Fixed | `dlg:Stepup-code` and `-locked` identical at 375 and 360. Cancel at x 591 in both at 1440 | Ghost description and ghost labels |
| B-05 | Fixed | CC: the §4.7 sentence is present on all 4 Data boards | |
| B-06 | Fixed | `hits:Preferences:390` and `1024touch`: segment 45, picker 49 | The rig's 30 px figure is a probe artefact (README) |
| B-07 | Fixed | Body 184/184 and 196/196 | |
| B-08 | Fixed | `RIG/shots/Stepup-code-locked/zoom200-1280x800-light.png`: footer hairline cue | Same cue on the crop and delete dialogs |
| B-09 | Fixed | `RIG/shots/Stepup/320x640-light.png`: the note balances on 2 lines | `.ra-detail` has `text-wrap: balance` |
| B-10 | Fixed | `pref:Preferences-offline:1440`: error at 1152, note at 1180 | |
| B-11 | Fixed | `dlg:Connected-remove:1440x900`: 142 and 142 px | |
| B-12 | Fixed | Timers 1:00 / 0:52 / 0:41; §4.0 "Countdowns start at 1:00" | |
| B-13 | Fixed | Slot is 40 px under a 480 px column and 60 px under 350. `inject.mjs`: no section moves at 768, 390 or 320 | |
| B-14 | Fixed | See A-17 | |
| C-01 | Fixed | See B-03 | |
| C-02 | Fixed | See A-04 | Residual: R-21 |
| C-03 | Fixed | See B-05 | |
| C-04 | Fixed (canvas) | CDP: link "Change email", description is the address (`P/Security.dc.html:43`). WCAG 2.5.3 passes | Differs from Revision 2's sr-suffix name: R-12 |
| C-05 | Fixed | `role="application"`, `aria-roledescription`, `aria-label` (`P/Profile-photo-crop.dc.html:24`) | Residual instruction text: R-14 |
| C-06 | Fixed | `P/Profile-email-code.dc.html:43` | |
| C-07 | **Not fixed** (in part) | (c) Remove photo: `is-focused` on Upload photo ✓. (b) Connected `h1` `tabindex=-1` ✓, but the empty title has none. (a) Locked: see B-02 | R-02, R-17 |
| C-08 | Moved to spec | §4.0 failure, offline and loading tables cover every page and Export | Covered |
| C-09 | Moved to spec | §8 row 13 maps to `RadioGroup` | Boards use `fieldset`, the spec says `div role=radiogroup`: R-20 |
| C-10 | Fixed | `PRB/aria/Main-1440.yml` and others: one sr string, `aria-hidden` halves, on the sidebar, index, Profile and Security rows and extreme | The new Export-request chip doesn't follow it: R-05 |
| C-11 | Moved to spec | §7.2 updated, with row 30 for utilities | 40+ classes are still unmapped: R-23 |
| C-12 | Moved to spec | §8 rows 7–9: "Don't use `ItemSeparator`" | Covered |
| C-13 | Fixed | `P/Profile-name-error.dc.html:59`: no role | |
| C-14 | Fixed | `P/Security-loading.dc.html:30` (status outside) and `:36` (`aria-busy` on the groups) | |
| C-15 | Moved to spec | §8 rows 10–11 | Covered |
| C-16 | Fixed | `form.ra-dialog-form` on both email steps and the Stepup-code family | Revision 2 widened the rule to every dialog: R-15 |
| C-17 | Fixed | No `aside`. The index has `nav aria-label="Account"` | Revision 2 wants the `nav` around all sidebar content: R-18 |
| C-18 | Fixed | `P/Account-index.dc.html:44`: spans only | |
| C-19 | Fixed | `<title>Profile, first name missing</title>` | |
| C-20 | Fixed | (a) The countdown is GrayText (`fc-colors.mjs`: button colour = GrayText `rgb(96,0,0)`). (b) Badge `border 0`, no outline. (c) `PRB/forced/Security-remove-passkey-mobile-390.png`: no full-width line. (d) Tails hidden, `PRB/forced/Main-1440x900.png` | |
| C-21 | Fixed | "Added March 12", "Connected August 14", "November 11" | Residual: R-21 (October 26 against 27) |
| C-22 | **Not fixed** | CC: "a code from your email", "sign-in codes" or "Verification code" on 32 boards | R-01 |
| C-23 | Moved to spec | §1.6.3 exempts provider names; §4.0 lists the ghost Dismiss | Covered |
| C-24 | Fixed | "Last name (optional)" on every Profile board | |
| C-25 | Fixed | `.ra-note` inside `.ra-section-foot`, no inline style (`P/Preferences.dc.html:146`) | |
| C-26 | Fixed | See A-17 | |
| C-27 | **Not fixed** | See the §6 checks | R-08, R-10 |
| C-28 | Moved to spec | §4.5 and §8 give option `lang` | The trigger value has no `lang`: R-20 |
| C-29 | Moved to spec | §4.0 toasts carry no action; inline Undo is drawn on `Profile-photo-removed` | Covered |
| C-30 | Fixed | Jay on saved and failed. Security-no-passkeys keeps the "Signed in with an email code" row | |
| C-31 | Moved to spec | §4.2 photo status slot, drawn on `Profile-photo-removed` | Covered |
| C-32 | Moved to spec | §7.4 initial-focus table and §4.0 "No close button". Crop area `is-focused` | Covered |

## New findings

| ID | Board(s) | Sev | What's wrong | Expected (§) | Evidence | Fix | Owner |
|---|---|---|---|---|---|---|---|
| R-01 | 32 boards: Main and all 11 Profile boards; Security and all 9 Security boards; all 6 Stepup boards; all 4 Preferences boards; Export-request-code | Major | **Revision 2's "email code" naming isn't on the canvas (C-22).** Security lede: "Sign in with a code from your email or with a passkey." Profile email row: "We send sign-in codes and account emails here." Preferences: "Sign-in codes and security alerts…" Stepup: "use a passkey or a code from your email." Remove passkey: "…or a code from your email." The code input's sr label is "Verification code, 8 letters and numbers" on 7 code boards | §4.0 one-name table; §4.2 Section B "We send email codes and security alerts here."; §4.3 lede "Sign in with an email code or a passkey. There's no password to forget."; §4.3 Remove "…or an email code."; §4.4 screen A "…use a passkey or an email code." and sr "Email code, 8 letters and numbers"; §4.5 "Email codes and security alerts. These can't be turned off." | CC; `P/Security.dc.html:28`, `P/Main.dc.html:128`, `P/Preferences.dc.html:111`, `P/Stepup.dc.html:21`, `P/Security-remove-passkey.dc.html:21`, `P/Stepup-code.dc.html:29`, `P/Export-request-code.dc.html:24` | Paste the §4 strings into the 6 base sources (Security, Main, Preferences, Stepup, Security-remove-passkey, the code label), then re-copy the full copies (§6.1) | canvas |
| R-02 | Stepup-code-locked | Major | **The locked state contradicts Revision 2.** The board puts `.is-focused` on the "Send a new code in 0:41" button. The error is `<p role="alert">` with no `tabindex` | §4.4 locked row: focus moves to the error message, `tabindex="-1"`, no `role="alert"`, and the board draws `is-focused` there. §7.4 initial-focus table and §5 visible-focus row say the same | `P/Stepup-code-locked.dc.html:35` (alert) and `:41` (`is-focused` on the button); CDP tree: `alert ""` | Give `#code-error` `tabindex="-1"` and `class="is-focused"`, drop `role="alert"`, and remove `is-focused` from the button. Or amend §4.4, §5 and §7.4 if the button is preferred, but pick one | canvas |
| R-03 | Stepup-code-locked (and the specified locked state of every code entry) | Major | **The locked code input is a Tab stop with no visible focus.** It is `readonly` and `aria-disabled`, so it stays in the Tab order (correct), but `.ra-otp.is-disabled` draws no ring on any slot. Shift+Tab from the error lands on an invisible focus (WCAG 2.4.7) | §7.4 one focus look on every focusable control; WCAG 2.4.7 | Rig major `focus/no-visible-focus-indicator #stepup-code` (`RIG/report.md`, kbd 1280 and 390, both schemes); `PRB/focus/Stepup-code-locked-1280-0.png` against `PRB/focus/Export-request-code-1280-0.png` | Draw the ring around the slot group on `.ra-otp.is-disabled:focus-within`, and state it in §7.2 row 20 | canvas, spec |
| R-04 | Stepup, Stepup-mobile, Stepup-code, -error, -error-dark, -locked | Major | **Step-up screens A and B have different heights**, so the title moves when you choose "Email me a code instead". At 1440: A is 334 px, B is 386 px, and the title moves 26 px. At 390 (drawer): A is 334, B is 470, and the title moves 136 px | §4.0 Dialogs: "step-up screens A and B and all their states" keep the height of the tallest state. §4.4 component paragraph | `PRB/probe-aria-geom.json` `dlg:Stepup:*` against `dlg:Stepup-code:*` | Give screen A the screen-B min-height (a ghost pane, as on the email dialog), or render A inside the same `.ra-steps` stack | canvas |
| R-05 | Export-request, Export-request-mobile | Major | **The account chip breaks the long-email rule.** The address is two flex spans with no sr string, so AT reads "jamie.rivera @northwind.coffee" (two words). This is the C-10 defect again, on a new board. With a long address the chip would also truncate or overflow without the sidebar rule | §4.8 chip "(long-email rule)"; §4.0 long emails; §7.2 row 15 markup | `P/Export-request.dc.html:24`; `PRB/aria/Export-request-1440.yml` ("paragraph: jamie.rivera @northwind.coffee") | Use the sidebar markup: `.ra-sr` full address plus `aria-hidden` local, `@` and domain spans | canvas |
| R-06 | Profile-photo-crop | Minor | **A-14 not fixed.** The crop frame doesn't shrink at short heights (`width: min(280px, 100%)`). At 844×390 and 1280×540 the Zoom slider is below the fold. The new footer hairline cue is drawn | §4.2 crop area and §7.2 row 26: `min(280px, 100svh - 360px)`, minimum 160 | `account.css:2341-2345`; `RIG/shots/Profile-photo-crop/844x390-light.png`; `dlg:Profile-photo-crop:844x390` body 376/176 | Apply the §7.2 row 26 size, keeping it square | canvas |
| R-07 | Main and every Profile board | Minor | **A-10 not fixed.** Two columns start at a 648 px column with a fixed 340 px form, so between 648 and 679 px the stage is 276–307 px (296 at a 668 px column, messenger 264). Between 620 and 647 px the layout stays single-column | §4.2 Section A and §7.2 row 24: two columns from 620 with `minmax(280px,340px) minmax(308px,1fr)`; the form shrinks first | `account.css:2644-2645`; `grid:Main:700` | Use the §7.2 row 24 grid | canvas |
| R-08 | Spec §6.2 | Minor | **C-27 not fixed.** The tables say they follow `canvas.json` but don't. 11 board heights are 100 px low (Main, Profile-dark, -name-saved, -save-failed and -extreme are 1000 on the canvas; Security, -dark, -passkey-waiting and -passkey-failed are 1600; -loading 1500; -no-passkeys 1100). Rows 4–8 are at y 4600, 5900, 7600, 8900 and 10200 in the spec, against 4700, 6000, 7700, 9000 and 10300 on the canvas. The title notes are 100 px off. The `row-profile` `maxW` is 15590 against 17110, and the spec says "Boards (50)" | §6.2; revision note "tables now follow canvas.json" | `canvas.json` boards and notes, compared row by row with the §6.2 tables. The canvas is self-consistent: `$preview` equals w×h on all 51, and the pitch rule holds | Regenerate §6.2 from `canvas.json` | spec |
| R-09 | Profile-photo-removed; spec §5, §6 | Minor | **The new board isn't in the spec.** §6.2 has no row for it (x 15670, 1440×1000, F, fill and int). §5 still says "'Photo removed.' with Undo: copy". §6.1 names `Profile-extreme` as the only own-sidebar board, but `Profile-name-saved` and `Profile-photo-removed` also carry sidebar copies, and §6.2 lists name-saved as importing `Account-sidebar`. The copies differ only in data (diffed: name, avatar) | §6.1 full copies and exceptions; §6.2; §5 | `canvas.json` row 1; `P/Profile-name-saved.dc.html:16-81`, `P/Profile-photo-removed.dc.html:16-81` | Add the row, cite the board in §5 (Saved; focus after action), and list the three own-sidebar boards in §6.1 | spec |
| R-10 | Deletion-pending, Export-request, -mobile, -code, Export-requested | Minor | **The board titles disagree three ways.** For example, Export-request-code is "Download your data, enter the code" in spec §6.2, "Data download after deletion, enter the code" in `canvas.json` and "Download your data after deletion, enter the code" in `<title>`. Deletion-pending: "Signing in, account scheduled for deletion" / "Account deletion, signed in during the 30 days" / "Account scheduled for deletion, signed in" | §6.1: the board `<title>` is the §6.2 board title | `canvas.json` row 8; `P/Export-request-code.dc.html:5` | Pick one set and apply it to all three places | spec, canvas |
| R-11 | Account-index, Account-index-dark | Minor | **The account row uses an sr prefix.** The name is "Profile: Jamie Rivera jamie.rivera@northwind.coffee". It still contains the visible label, so WCAG 2.5.3 passes, but it breaks the Revision 2 rule, and the revision note says the row is fixed | §4.1 account row: sr suffix ", profile"; §4.0 "Use a sr-only suffix, never a prefix" | `P/Account-index.dc.html:28`; `PRB/aria/Account-index-390.yml` | `<p data-slot="item-title">Jamie Rivera<span class="ra-sr">, profile</span></p>` | canvas |
| R-12 | Security and all 7 boards that show its Email codes row | Minor | **"Change email" is named differently from the spec.** The board's name is "Change email", with the address as its description. That passes WCAG 2.5.3, but the spec dictates "Change email, where your codes go" | §4.3 Email codes row; §4.0 SR rules | `P/Security.dc.html:43`; CDP tree | Add the sr suffix as specified, or amend §4.3 to the `describedby` pattern | canvas or spec |
| R-13 | Profile-photo-removed | Minor | **Undo has no context.** Its accessible name is "Undo" | §4.2 photo states, Removed: sr name "Undo, restore your photo" | `P/Profile-photo-removed.dc.html:114`; CDP: `button "Undo"` | Add `<span class="ra-sr">, restore your photo</span>` | canvas |
| R-14 | Profile-photo-crop | Minor | **C-05 residual.** The keyboard help reads "Use the arrow keys to move it.", and `aria-describedby` points only at it, not at the dialog description | §4.2 crop row: `aria-describedby` = the description plus "With a keyboard, use the arrow keys to move your photo, plus and minus to zoom, and Home to center it." | `P/Profile-photo-crop.dc.html:24,28` | Use the §4.2 sentence and set `aria-describedby="crop-desc crop-help"` | canvas |
| R-15 | Stepup (screen A), Profile-photo-crop, Security-remove-passkey (+mobile), Connected-remove, Data-delete-confirm (+mobile) | Minor | **Not every dialog is a `<form>`.** Screen A, the crop dialog and both confirms have no form. "Save photo" is `type="button"`. Data-delete-confirm uses an empty `<form hidden>` with `form=` attributes on the input and the commit, instead of a form around the body and footer. That works for Enter, but it isn't the pattern | §4.0 Dialogs: "Every dialog is a `<form>` around its body and footer"; §8 row 18 | `P/Stepup.dc.html:17-32`; `P/Profile-photo-crop.dc.html:37`; `P/Data-delete-confirm.dc.html:19,33,39` | Wrap the body and footer in `form.ra-dialog-form` as on Stepup-code, with the commit as `type="submit"` | canvas |
| R-16 | Export-request-code | Minor | **The code meta isn't reserved for the error at phone widths.** On the solo page it stays at 2 lines (44 px), because the 3-line rule is a container query on the dialog body. At 320 the wrong-code error wraps, and "Request download" moves down 20 px | §1.5 rule 8 (code meta 3 lines below a 360 px container); §4.4 reserved lines; §4.8 "The §4.4 code rules" | `inject.mjs` at 320: meta 44 → 64, button +20; `solo:Export-request-code:320x640` meta 44 | Put a size container on the solo form so the same `max-width: 359px` rule applies | canvas |
| R-17 | Connected-empty | Minor | **The empty-state title isn't a focus target.** This board is the state right after removing the last app, where focus lands on the empty title. It has no `tabindex="-1"` | §4.6 Then; §4.0 SR rules; §7.4 after an action (C-07 b) | `P/Connected-empty.dc.html:34` | `<h2 data-slot="empty-title" tabindex="-1">` | canvas |
| R-18 | Account-sidebar and its 3 local copies (every desktop page) | Minor | **The `nav` wraps only the menu.** The back link, the name and email header, Help and Sign out sit outside any landmark | §3.2 and §4.1: one `<nav aria-label="Account">` wraps everything in the container, so no content is outside a landmark; §8 rows 2–3 | `P/Account-sidebar.dc.html:16-65` | Move the `nav` open tag up to wrap the whole container content | canvas |
| R-19 | Data, Data-mobile, Data-dark, Data-ready (and the delete overlays) | Minor | **The delete section has no status slot.** "Delete account" sits in `.ra-inline` with no foot or slot | §4.7: "Delete account … follows the description, then the status slot" | `P/Data.dc.html:57-59`; `data:Data:1440` (delete foot null) | Add a `.ra-section-foot` with an empty slot, or drop the slot from §4.7, because failures show inside the dialog | canvas or spec |
| R-20 | Preferences, -mobile, -dark, -offline | Minor | **Segmented markup differs from the spec.** The boards use `<fieldset>` with an sr `<legend>`, while the spec specifies `div role="radiogroup" aria-labelledby={row title}` (C-09 residual). The language trigger value has no `lang="en-US"` (C-28 residual). Both are accessible as built | §4.5 Appearance and Language rows; §7.2 row 13 | `P/Preferences.dc.html:42-43,55,92` | Either amend §4.5 and §7.2 row 13 to the fieldset pattern (my recommendation), or change the boards. Add `lang` to `#language-value` | spec, canvas |
| R-21 | Main and every Profile board; Security family; Data-deleted | Minor | **Small copy differences:** the customer bubble reads "Hi, can I change…" (spec "Hi! Can I change…"); Firefox on Windows reads "Last active October 26." (spec "October 27."); Data-deleted links `#sign-in` (spec `#signin`) | §4.2 preview table; §4.3 sessions table; §4.8 Data-deleted link | `P/Main.dc.html:89`; `P/Security.dc.html:117`; `P/Data-deleted.dc.html:23` | Match the spec (or amend it) | canvas |
| R-22 | Every page with section status slots | Minor | **The status slot reserves 1 line (20 px) when the column is 480 px or wider**, 2 lines below 480 and 3 below 350. The spec says 2 lines (40 px) at every width. The §4.2 "Email changed. We'll send codes to jamie@rivera.studio…" status wraps to 2 lines at 1440. Email is the last section, so nothing moves today, but any 2-line status in a middle section would | §1.5 rule 8; §4.0 status slot "reserves 2 lines (40 px) even when it is empty" | `account.css:583-586,2661-2667`; `inject.mjs` (Main email 20 → 40 at 1440) | Document the width-based rule in §1.5, §4.0 and §7.2 row 16, or reserve 40 px everywhere | spec |
| R-23 | `account.css`; spec §7.2, §8 | Minor | **Recipes the spec doesn't describe (C-11 residual).** Behaviour-bearing but unmapped: `.ra-steps`, `.ra-step-ghost` and `.ra-step-busy` (the one-height dialog and commit min-width mechanism); `.ra-dialog-form` and `.ra-dialog-pane`; `.ra-form-alert` (the 40 px alert slot); `.ra-photo-slot`, `.ra-photo-status`, `.ra-photo-actions` and `.ra-photo-row` (photo slot, inline Undo, the 320 reserve); `.is-even` (equal-width dialog footer, used on Data-delete-confirm, which contradicts "dialog footers are right-aligned" in §1.5 rule 5 and §7.2 row 18); `.ra-skel-*` and `.ra-w40/50/60`; `.ra-trunc`; `.ra-bar`. Dead CSS: `.ra-p`, `.ra-strong`, `.ra-toast-info`. §7.2 row 28 says the chip "contains the code entry on Export-request-code", but that board has no chip | §7.2 intro ("where the built account.css and this table differ, this table wins"); §7.2 row 30; §8 | `account.css:1564-1569,2554-2556`; class audit in method | Add the rows to §7.2 and §8 (or a "canvas mechanics" note), decide the delete footer layout, and delete the dead rules | spec, canvas |

## Rig summary after tuning

| Run | Blocker | Major | Minor |
|---|---|---|---|
| Untuned rig (same build, `UNT/report.md`) | 20 | 1137 | 1521 |
| **Tuned rig** (`RIG/report.md`) | **20** | **85** | **1483** |

The tuning removed 994 overlap majors (inert page text behind every overlay), 20 clipped-text majors inside `aria-hidden` drawings, 38 focus majors (drawn `.is-focused` rings) and 38 axe "needs review" items on inert content.

**What's left, by cause:**

| Rig output | Count | Verdict |
|---|---|---|
| Blocker: horizontal overflow on `.fixed` 390 wrappers at 320 and 375 | 20 | False positive: fixed 390 frame. The fluid variants don't overflow |
| Major: clipped or overlapping text on `.fixed` mobile wrappers at ≥768 (Preferences-mobile 35, Security-mobile 23, Connected-mobile 8, Data-mobile 4, Profile-mobile 1) | 71 | False positive: the desktop shell inside a 390 div |
| Major: Profile-extreme sidebar name clipped | 4 | False positive: the intended 2-line clamp with an ellipsis (§3.2) |
| Major: Data-delete-confirm(-mobile) list item overlaps the footer at 844×390 | 6 | False positive: the text is scrolled out of view under the sticky footer, and the cue is drawn |
| Major: Connected-empty toast over the empty description at 844×390 | 2 | By design: the toast overlays content |
| Major: `#stepup-code` no visible focus (Stepup-code-locked) | 2 | **Real**: R-03 |
| Minor: targets under 44 px (sidebar items and `sm` buttons on fine pointers, inert page buttons, segmented radios probed on the stretched input, solo footer links ≥24 on fine pointers, switches 43 by rounding) | 1324 | Accepted by §7.3, or a probe artefact. I verified phone hit areas separately: segments 45, picker 49, slider 44, plain buttons 44, footer links 44 |
| Minor: inline "Privacy Policy" under 24 px | 19 | Exempt (WCAG 2.5.8) |
| Minor: axe "needs review" colour contrast | 140 | Text on the dialog over the scrim, pseudo-element backgrounds, empty inputs. Dark harness: 0 violations |

## Forced colours and reduced motion

**Forced colours** (Chromium `forcedColors: active`): `Main`, `Security`, `Stepup-code-locked`, `Preferences`, `Data-delete-confirm` and `Export-request-code` at 1440×900 and 390×844, plus `Connected-remove` and `Security-remove-passkey-mobile` at 390 and 1440 (`PRB/forced/`). axe found 0 violations in all 12 runs.

- **Focus:** a 2 px Highlight outline on every stop tabbed, the first 4 per board (`*-focus0..3.png`).
- **Exception:** the OTP inputs draw focus on the active slot. In the locked state nothing draws (R-03).
- **Main:** the sidebar rule survives (A-16). Bubble tails are hidden and the borders are continuous (C-20d). The preview loses the Signal fill, as §7.7 expects.
- **Stepup-code-locked:** the disabled slots and the "Send a new code in 0:41" button are GrayText (C-20a). The error keeps its icon.
- **Preferences:** the selected segment and the checked switches are Highlight, the unchecked switches are ButtonText. Separators become GrayText.
- **Security:** the "This device" badge has `border 0` and no outline, so it isn't clipped (C-20b).
- **Action sheets:** only the visible pieces are outlined, with no full-width line (C-20c).
- **Data-delete-confirm and Export-request-code:** the field, slots and buttons keep their borders. The shiba keeps its colours (an image, as expected).

**Reduced motion:** 9 boards (the six above plus `Security-passkey-waiting`, `Security-loading` and `Stepup-code`), comparing `no-preference` with `reduce` (`PRB/probe-dark-forced-motion-focus.json` `motion`).

- All transitions go to 0 s.
- The spinner goes from 0.8 s to 2.4 s and keeps turning.
- The skeleton `ra-pulse` and the caret `ra-blink` are removed.
- Only scroll-linked animations remain: the dialog edge cues and the email ellipsis, which are not time-based. This matches §7.6.
- Dialog, drawer and toast entrance motion can't be observed on static boards.

## New and changed boards: full review

All copy below was checked verbatim against §4.7 and §4.8 (CC). Visual review: 1440, 768, 390 and 320 in light, harness dark and drawn dark, plus 320×480, 844×390 and 1280×540 for the dialogs.

- **Data, Data-mobile, Data-dark, Data-ready:**
  - The lede, the download description and the delete description match §4.7.
  - Data-ready's Download is a default `sm` link with the name "Download your data, 2.4 MB zip file", which starts with the label.
  - The download status slot is reserved (20 / 40 / 60).
  - Open: R-19.
- **Data-delete-confirm and -mobile:**
  - The title, lede, 4 list items, label, prefilled value and footer match §4.7.
  - `aria-describedby` covers the lede and the list. The typed field has the `is-focused` ring, and an error slot is reserved under it.
  - At 390 it is a drawer with a handle, the commit on top and a centred header. At 320×640 and 844×390 the body scrolls with the footer cue.
  - Harness dark is correct: the destructive tint is legible and the ring is `#2997ff`.
  - Open: R-15, and the `.is-even` footer (R-23).
- **Data-deleted:**
  - Title, body, detail and link match §4.8.
  - The shiba is 93×117 at ≥768 and 62×78 below, pixelated, `alt=""`.
  - The `h1` has `tabindex=-1`. The footer is a `contentinfo` landmark. The link's hit area is 44 on phones.
  - Open: R-21 (`#sign-in`).
- **Deletion-pending:**
  - Copy matches §4.8.
  - Keep my account (default) and Sign out (outline) are block `lg` buttons, with a status slot under them.
  - Open: R-10 (title).
- **Export-request and -mobile:**
  - Copy matches §4.8. The button is `aria-describedby` the note. There is no "Not you?" link.
  - The chip fits at 320 (284 of 288 px).
  - Open: R-05, R-10.
- **Export-request-code:**
  - Copy matches §4.8. There are 8 slots (4 + 4): 44×52, then 38×44 at 390 and 29×44 at 320.
  - "Codes expire after 5 minutes." and "Send a new code in 1:00" are shown, with a polite sr status. The commit is `type="submit"`.
  - The active slot draws real focus.
  - Open: R-01 (sr label), R-16, R-10.
- **Export-requested:**
  - Copy matches §4.8.
  - No action, as specified; only the footer links.
  - Open: R-10.
- **Profile-photo-removed:**
  - Initials "JR" in the sidebar, the photo row and the preview. "Photo removed." (polite, success icon) is followed by a plain Undo outside the live paragraph.
  - Upload photo has `is-focused`. The hint is `hidden` but still describes Upload photo.
  - The photo row holds at 112 (134 at ≤375), so the form doesn't move.
  - The sr preview sentence says "initials". The spec has no wording for the no-photo case, and this is reasonable.
  - Open: R-13, R-09, R-01 (inherited email-row copy), R-21.

## Spec ↔ canvas consistency after Revision 2

- **Copy (§4):** `copycheck.py` checked every spec string per board. All differences are in R-01, R-11, R-12, R-13, R-14 and R-21. Everything else matches verbatim, including all of §4.6, §4.7 and §4.8.
- **Board list and sizes (§6):** see R-08, R-09 and R-10. `canvas.json` itself is consistent:
  - 51 boards and 51 files, with `order` equal to the board keys;
  - `$preview` equals w×h on every board;
  - imports match §6.2 except the own-sidebar copies (R-09);
  - row pitch is the tallest board + 400, and note `maxW` values equal the row spans.
- **CSS (§7 and §8):** 48 of the 128 classes in `account.css` are named nowhere in §7–§8. The behaviour-bearing ones are in R-23.
- **States specified as drawn but missing or different:**
  - locked focus on the error message (R-02);
  - the chip's long-email markup (R-05);
  - Undo's sr name (R-13);
  - the delete status slot (R-19);
  - screen A at screen B's height (R-04);
  - the crop frame's short-height size (R-06).

## Per-board result

Fail means at least one open Major applies, including one inherited through an import, as in Phase 4. Minors are listed per board where they are specific to it. R-01 alone accounts for 26 of the 34 fails, so a single copy pass would clear most of them.

| Board | Result | Open findings |
|---|---|---|
| `Main` | Fail | R-01; R-07, R-21 |
| `Profile-mobile` | Fail | R-01; R-07, R-21 |
| `Profile-dark` | Fail | R-01; R-07, R-21 |
| `Profile-name-saved` | Fail | R-01; R-07, R-21 |
| `Profile-name-error` | Fail | R-01; R-07, R-21 |
| `Profile-name-error-dark` | Fail | R-01; R-07, R-21 |
| `Profile-save-failed` | Fail | R-01; R-07, R-21 |
| `Profile-photo-crop` | Fail | R-01 (page behind); R-06, R-14, R-15 |
| `Profile-email-change` | Fail | R-01 (page behind) |
| `Profile-email-code` | Fail | R-01 (sr label, page behind) |
| `Profile-extreme` | Fail | R-01; R-07, R-21 |
| `Profile-photo-removed` | Fail | R-01; R-07, R-09, R-13, R-21 |
| `Account-sidebar` | Pass with minors | R-18 |
| `Account-index` | Pass with minors | R-11 |
| `Account-index-dark` | Pass with minors | R-11 |
| `Security` | Fail | R-01; R-12, R-21 |
| `Security-mobile` | Fail | R-01; R-12, R-21 |
| `Security-dark` | Fail | R-01; R-12, R-21 |
| `Security-loading` | Fail | R-01 |
| `Security-no-passkeys` | Fail | R-01; R-12 |
| `Security-passkey-waiting` | Fail | R-01; R-12, R-21 |
| `Security-passkey-failed` | Fail | R-01; R-12, R-21 |
| `Security-remove-passkey` | Fail | R-01; R-15 |
| `Security-remove-passkey-mobile` | Fail | R-01; R-15 |
| `Stepup` | Fail | R-01, R-04; R-15 |
| `Stepup-mobile` | Fail | R-01, R-04 |
| `Stepup-code-error-dark` | Fail | R-01, R-04 |
| `Stepup-code` | Fail | R-01, R-04 |
| `Stepup-code-error` | Fail | R-01, R-04 |
| `Stepup-code-locked` | Fail | R-01, R-02, R-03, R-04 |
| `Preferences` | Fail | R-01; R-20 |
| `Preferences-mobile` | Fail | R-01; R-20 |
| `Preferences-dark` | Fail | R-01; R-20 |
| `Preferences-offline` | Fail | R-01; R-20 |
| `Connected` | Pass | none |
| `Connected-mobile` | Pass | none |
| `Connected-dark` | Pass | none |
| `Connected-remove` | Pass with minors | R-15 |
| `Connected-empty` | Pass with minors | R-17 |
| `Data` | Pass with minors | R-19 |
| `Data-mobile` | Pass with minors | R-19 |
| `Data-dark` | Pass with minors | R-19 |
| `Data-ready` | Pass with minors | R-19 |
| `Data-delete-confirm` | Pass with minors | R-15, R-19 (page behind), R-23 (`.is-even`) |
| `Data-delete-confirm-mobile` | Pass with minors | R-15, R-23 |
| `Data-deleted` | Pass with minors | R-21 |
| `Deletion-pending` | Pass with minors | R-10 |
| `Export-request` | Fail | R-05; R-10 |
| `Export-request-mobile` | Fail | R-05; R-10 |
| `Export-request-code` | Fail | R-01 (sr label); R-10, R-16 |
| `Export-requested` | Pass with minors | R-10 |

**Totals:** 34 fail, 17 pass (14 with minors).

- R-18 (the sidebar `nav` scope) applies to every board that shows the desktop sidebar, so it is listed once, on `Account-sidebar`.
- R-08, R-22 and R-23 are spec-wide and apply to no single board.
