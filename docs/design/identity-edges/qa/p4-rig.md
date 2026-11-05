# WP4.1 rig sweep (Phase 4, identity edges)

Tester: WP4.1. Canvas version 1793888623-5e41 (local mirror `.design/edges/project/`), 73 boards, 2026-11-05. No board, stylesheet or app file was edited.

## 1. Run summary

| Run | Boards | Blockers | Majors | Minors | Results |
|---|---|---|---|---|---|
| 1. Light build, default viewports (320 to 1920, 200% zoom) x light and dark scheme | 73 | 0 | 0 | 564 | `.design/edges/qa/p4-rig/light/report.md` |
| 2. Forced-dark build (`BOARD_THEME=dark`), `--schemes dark` | 73 | 0 | 0 | 564 | `.design/edges/qa/p4-rig/dark/report.md` |
| 3. `--forced-colors`, light and dark | 73 | 0 | 807 (all axe colour-contrast) | 644 | `.design/edges/qa/p4-rig/forced/report.md` |
| 4. Drift | 1 shared element group with drift | | | | RIG-04 |
| 4. Icons (`check-icons.py`, run with `.design/edges/icons`) | all boards | `bad: 0` | | | |
| 5. Layout shift (Playwright, 390x844 and 1440x900) | 8 flows, 31 state boards | see section 3 | | | script in scratch, table below |
| 6. Manual: text spacing (WCAG 1.4.12) at 320 and 1440, extra to the brief | 73 | 4 findings | | | RIG-01 to RIG-03, RIG-09 |

The default rig run found nothing above minor. Every real finding below comes from the extra checks (text spacing, layout shift, drift, screenshot review).

### Discarded as known false positives or by-design items

| Category | Run 1 / Run 2 (each) | Run 3 |
|---|---|---|
| Sidebar items 36 px on fine pointers (Account-sidebar, Profile*, Security*, Session-expired*) | 224 | in the 506 target-size minors |
| Footer links 24 px on fine pointers (`.ri-foot`, `.ri-rec-mail-foot`) | 153 | same |
| Inline links under 44 px (WCAG 2.5.8 exempt) | 40 | same |
| Buttons on the `inert` page behind overlays (32 and 36 px row buttons, Change photo, Link/Unlink, Rename, Remove; Security-rename, -rename-error-mobile, -remove-failed, -unlink, Session-expired*, Profile-email-taken*, Profile-email-code-mobile) | 100 | same |
| Email-recovery-alert footer links (24 px, email footer) | 12 | same |
| axe "needs review" colour-contrast (overlay pages, `#code`, `#link-code`, `button[aria-label="Link Microsoft"]`; spot-checked: `.is-error` 5.70 light / 6.61 dark, `#limit-status` 8.16 dark) | 35 | 115 |
| axe colour-contrast majors under `--forced-colors` (axe measures authored colours against forced backgrounds; verified on screenshots instead) | | 807 |
| Total | 564 | 807 majors + 644 minors |

Also not findings: `aria-disabled` buttons drawn muted (Limited "Try again in 12:00", Recover-declined "Start a new request on November 12", Signin-limited countdown): contrast-exempt. Alert icon in forced-dark is authored red `rgb(208,0,10)` on black (3.7:1, above 3:1 for graphics) and is `aria-hidden`.

Keyboard: no `no-visible-focus-indicator` major on any board in runs 1 to 3; Recover-code and Provider-link-code report the drawn `.is-focused` slot.

## 2. Findings

| ID | Severity | Board(s) | Viewport/theme | Criterion | Evidence | Suggested fix | Owner |
|---|---|---|---|---|---|---|---|
| RIG-01 | blocker | Recover-declined, Recover-limited | 320 px, light and dark, with WCAG 1.4.12 text spacing (line-height 1.5, letter-spacing .12em, word-spacing .16em, paragraph spacing 2em) | 1.4.12 Text Spacing, 1.4.10 Reflow (horizontal scroll at <=428) | Normal rendering has no overflow. With spacing, page `scrollWidth` is 334 (Recover-declined) and 325 (Recover-limited) at 320. Cause: block buttons are `white-space: nowrap` (computed on every `[data-slot=button]`); label widths 347 ("Start a new request on November 12") and 329 ("Send request tomorrow at 9:15 …") against a 288 px button. Screenshots: `/private/tmp/claude-501/-Users-akakijomidava-orca-workspaces-Richie-cetacean/300c9cb1-b437-49c8-ae7f-96f52969e541/scratchpad/ts-Recover-declined.png`, `/private/tmp/claude-501/-Users-akakijomidava-orca-workspaces-Richie-cetacean/300c9cb1-b437-49c8-ae7f-96f52969e541/scratchpad/ts-Recover-limited.png`. | Let block buttons wrap: `white-space: normal; height: auto; min-height: 44px; padding-block` for `[data-block]` in the identity frame (or shorten to "Start a new request on Nov 12" and keep wrapping as the fallback). Then re-test at 320 with the spacing style. | Shared CSS (PM) (fix); boards WP3.4 |
| RIG-02 | major | Provider-no-account, Provider-no-email, Signup-finish, Signup-finish-mobile | 320 px, text spacing | 1.4.12 (text spills out of the button) | Same `nowrap` cause. Label vs 288 px button: "Use a different Google account" 293 (Provider-no-account, Signup-finish), "Create an account with Google" 289, "Sign in with your email instead" 291 and "Use a different Microsoft account" 316 (Provider-no-email). Text runs past the border; no page overflow. | Same fix as RIG-01. | Shared CSS (PM); boards WP3.5 |
| RIG-03 | major | Security-passkey-added | 320 px, text spacing | 1.4.12 (clipped text) | The new passkey's name line (`p.wrap-2`, "Added the passkey “YubiKey 5 S…”") is a 2-line clamp: `scrollHeight` 72 vs `clientHeight` 48, so the third line is cut. Screenshot: `/private/tmp/claude-501/-Users-akakijomidava-orca-workspaces-Richie-cetacean/300c9cb1-b437-49c8-ae7f-96f52969e541/scratchpad/ts-Security-passkey-added.png`. Not seen without spacing. | Drop the clamp on narrow widths or give the row `overflow: visible` with `-webkit-line-clamp: none` under `@media (max-width: 428px)`; keep the clamp on desktop only if the full name is available another way (title, rename dialog). | WP3.3 settings |
| RIG-04 | minor | Account-sidebar (vs Profile-email-changed, Security-recovered and the Profile/Security copies) | all, light and dark | Consistency (Part A 4.12 sample data) | `node .design/qa-rig/drift.mjs`: `nav.ra-side-nav` differs in Account-sidebar: `jamie@rivera.studio` and `.ri-email-short` markup against `jamie.rivera@northwind.coffee` in the other copies. Spec 4.12 fixes the sample to northwind.coffee. | Update Account-sidebar to the 4.12 sample address (or confirm the shared part is unused as a visible board). | Shared CSS (PM) (foundation board) |
| RIG-05 | minor | Security-linked, Security-link-error | 390 and 1440 | Part A 1.6/4.1 (states don't move the page) | The section status slot is not reserved: Google and Microsoft section is 308 px high in Security-linked against 288 in Security, so "Where you're signed in" and "Recent security activity" `h2` move down 20 px at 1440 (0 at 390, where the line fits the reserved space); Security-link-error moves them 48 px at 1440 and 68 px at 390. Everything above the section (h1, Email codes, Passkeys) is unmoved. | Reserve a 20 px status slot under the section foot in Security (as `.ri-slot` does on sign-in), so the linked state only fills it. The two-line error can stay an accepted push. | WP3.3 settings |
| RIG-06 | minor | Security-recovered | 390 and 1440 | Spec R14 says "full copy of Security + alert + activity entry" | Section heights differ from Security beyond the alert: Passkeys 1 row (256 px) vs 2 (352), Where you're signed in 1 row (192) vs 3 (388), Google and Microsoft 268 vs 288, activity 4 rows. Result: Passkeys `h2` +148, "Where you're signed in" +32, "Recent security activity" -164 against the base at 1440. Might be intended (recovery signs out other devices) but it is not in the spec text, and the board can't be used as a shift reference. | Confirm in spec-recovery that post-recovery shows 1 passkey and 1 session and why Google is shorter, or redraw from Security with only the alert and activity entry added. | WP3.4 recovery |
| RIG-07 | minor | Signedout-prompt, Signedout-everywhere, Error-link, Recover-code, Recover-details, Provider-link-code, Provider-no-account, Provider-no-email, Signup-finish | 390 and 1440 | Part A 1.6 rule 5 / 4.1 (primary stays put between a page and its states) | Mascot top is 64 (32 at 390) and `h1` top is 166 (134) on every page without a step line, 190 (158) with one: no mascot or h1 move anywhere. The first filled button does move between different pages of a family because content above it differs: Error-link -24 (1440) / +12 (390), Recover-code +16 / +28, Signedout-prompt +44 / +68, Signedout-everywhere +36 at 390 only (h1 wraps to 2 lines), Provider-link-code +88 / +100, Provider-no-email -72, Signup-finish -32 / +4. All are content-driven, not state-driven; the true state boards (Signin-redirecting, -offline, -limited, Signin-provider-*, Security-*) shift only as documented. | PM to say whether the rule covers whole page families or only a page and its own states. If families: pin the button to the lede-bottom + one slot, or accept this and note it in Part A 1.6. | Shared CSS (PM) |
| RIG-08 | minor | Main, Error-link, Provider-link, Signup, Signedout-prompt, Recover-approved (all boards) | forced colours, light and dark | 1.4.11 / hierarchy under forced colours | In forced colours every button is a ButtonText outline, so the one filled primary looks the same as the outline secondaries (`.design/edges/qa/p4-rig/forced/shots/Main/1440x900-light.png`, `.design/edges/qa/p4-rig/forced/shots/Error-link/1440x900-light.png`). Focus rings, borders, badges and marks all survive; only primary/secondary hierarchy is lost. | In `@media (forced-colors: active)`, give `[data-variant=default]` a 2 px `ButtonText` border or a `Highlight` background with `HighlightText`. | Shared CSS (PM) |
| RIG-09 | minor | Account-sidebar and every Security*/Profile* board with the sidebar | 1440, text spacing | 1.4.12 (truncated text) | The "Sign-in and security" nav item truncates with an ellipsis (`scrollWidth` > `clientWidth`) once letter-spacing is .12em; the other items fit. | Allow the nav label to wrap (`white-space: normal`, `min-height: 36px`) or widen the sidebar to 272 px. | Shared CSS (PM) |

Findings per severity: blocker 1, major 2, minor 6 (9 in total).
Findings per owner: Shared CSS (PM) 6 (RIG-01, -02, -04, -07, -08, -09), WP3.3 settings 2 (RIG-03, -05), WP3.4 recovery 1 (RIG-06), WP3.2 edge 0, WP3.5 SSO 0 (boards affected by the shared RIG-01/-02 fix: WP3.4 and WP3.5).

## 3. Layout-shift table

Method: Playwright against the light build (`out-p4/*.html`, fluid), 390x844 and 1440x900. Values are Δx/Δy in px of the element's border box against the base board, in page coordinates; `0` means under 0.05 px. Elements: mascot (`.ri-mark`), `h1`, first `[data-variant=default]` button. Security rows compare `h1` and every section `h2` (an `h2` that moves is below the changed section unless stated). `n/a` means the board has no such element.

Result: mascot and `h1` never move in any flow (all 0/0). Moves of the first filled button: documented alert exceptions (Signin-provider-canceled, Signin-provider-error), and content-driven moves between different pages (RIG-07). Security sections move only below the changed section (RIG-05, RIG-06).

| Flow (base) | State board | Element | Δx/Δy at 390 | Δx/Δy at 1440 | Note |
|---|---|---|---|---|---|
| Main | Signin-redirecting | mark | 0/0 | 0/0 |  |
| Main | Signin-redirecting | h1 | 0/0 | 0/0 |  |
| Main | Signin-redirecting | first filled button | 0/0 | 0/0 |  |
| Main | Signin-provider-canceled | mark | 0/0 | 0/0 | load-time alert pushes form (Part A 4.10, documented) |
| Main | Signin-provider-canceled | h1 | 0/0 | 0/0 |  |
| Main | Signin-provider-canceled | first filled button | 0/+164 | 0/+140 |  |
| Main | Signin-provider-error | mark | 0/0 | 0/0 | load-time alert pushes form (4.10, documented) |
| Main | Signin-provider-error | h1 | 0/0 | 0/0 |  |
| Main | Signin-provider-error | first filled button | 0/+228 | 0/+180 |  |
| Main | Signin-offline | mark | 0/0 | 0/0 |  |
| Main | Signin-offline | h1 | 0/0 | 0/0 |  |
| Main | Signin-offline | first filled button | 0/0 | 0/0 |  |
| Main | Signin-limited | mark | 0/0 | 0/0 |  |
| Main | Signin-limited | h1 | 0/0 | 0/0 |  |
| Main | Signin-limited | first filled button | 0/0 | 0/0 |  |
| Signedout | Signedout-prompt | mark | 0/0 | 0/0 | different page (E11): lede + account chip above the buttons |
| Signedout | Signedout-prompt | h1 | 0/0 | 0/0 |  |
| Signedout | Signedout-prompt | first filled button | 0/+68 | 0/+44 |  |
| Signedout | Signedout-everywhere | mark | 0/0 | 0/0 | different page: h1 wraps to 2 lines at 390 (+36), copy-driven |
| Signedout | Signedout-everywhere | h1 | 0/0 | 0/0 |  |
| Signedout | Signedout-everywhere | first filled button | 0/+36 | 0/0 |  |
| Timeout | Error | mark | 0/0 | 0/0 | same y: no shift |
| Timeout | Error | h1 | 0/0 | 0/0 |  |
| Timeout | Error | first filled button | 0/0 | 0/0 |  |
| Timeout | Error-link | mark | 0/0 | 0/0 | different page: lede length (and h1 wrap at 390) |
| Timeout | Error-link | h1 | 0/0 | 0/0 |  |
| Timeout | Error-link | first filled button | 0/+12 | 0/-24 |  |
| Timeout | Error-notfound | mark | 0/0 | 0/0 |  |
| Timeout | Error-notfound | h1 | 0/0 | 0/0 |  |
| Timeout | Error-notfound | first filled button | 0/0 | 0/0 |  |
| Timeout | Offline | mark | 0/0 | 0/0 |  |
| Timeout | Offline | h1 | 0/0 | 0/0 |  |
| Timeout | Offline | first filled button | 0/0 | 0/0 |  |
| Timeout | Limited | mark | 0/0 | 0/0 |  |
| Timeout | Limited | h1 | 0/0 | 0/0 |  |
| Timeout | Limited | first filled button | 0/0 | 0/0 |  |
| Recover-account | Recover-email | mark | 0/0 | 0/0 |  |
| Recover-account | Recover-email | h1 | 0/0 | 0/0 |  |
| Recover-account | Recover-email | first filled button | 0/0 | 0/0 |  |
| Recover-account | Recover-code | mark | 0/0 | 0/0 | different step page: step content above the button |
| Recover-account | Recover-code | h1 | 0/0 | 0/0 |  |
| Recover-account | Recover-code | first filled button | 0/+28 | 0/+16 |  |
| Recover-account | Recover-details | mark | 0/0 | 0/0 | different step page: 5 fields above the button |
| Recover-account | Recover-details | h1 | 0/0 | 0/0 |  |
| Recover-account | Recover-details | first filled button | 0/+576 | 0/+576 |  |
| Recover-review | Recover-approved | mark | 0/0 | 0/0 |  |
| Recover-review | Recover-approved | h1 | 0/0 | 0/0 |  |
| Recover-review | Recover-approved | first button (base has no filled one) | 0/0 | 0/0 |  |
| Recover-review | Recover-completed | mark | 0/0 | 0/0 | base has an outline-only primary; compare first button |
| Recover-review | Recover-completed | h1 | 0/0 | 0/0 |  |
| Recover-review | Recover-completed | first button (base has no filled one) | 0/-288 | 0/-252 |  |
| Recover-review | Recover-declined | mark | 0/0 | 0/0 |  |
| Recover-review | Recover-declined | h1 | 0/0 | 0/0 |  |
| Recover-review | Recover-declined | first button (base has no filled one) | 0/-252 | 0/-252 |  |
| Recover-review | Recover-canceled | mark | 0/0 | 0/0 |  |
| Recover-review | Recover-canceled | h1 | 0/0 | 0/0 |  |
| Recover-review | Recover-canceled | first button (base has no filled one) | 0/-288 | 0/-252 |  |
| Recover-review | Recover-cancel | mark | 0/0 | 0/0 |  |
| Recover-review | Recover-cancel | h1 | 0/0 | 0/0 |  |
| Recover-review | Recover-cancel | first button (base has no filled one) | 0/0 | 0/0 |  |
| Provider-link | Provider-link-code | mark | 0/0 | 0/0 | different page: code slots + links above the button |
| Provider-link | Provider-link-code | h1 | 0/0 | 0/0 |  |
| Provider-link | Provider-link-code | first filled button | 0/+100 | 0/+88 |  |
| Provider-link | Provider-no-account | mark | 0/0 | 0/0 | different page: content above the button differs |
| Provider-link | Provider-no-account | h1 | 0/0 | 0/0 |  |
| Provider-link | Provider-no-account | first filled button | 0/-4 | 0/-4 |  |
| Provider-link | Provider-no-email | mark | 0/0 | 0/0 | different page: no email chip/field above the button |
| Provider-link | Provider-no-email | h1 | 0/0 | 0/0 |  |
| Provider-link | Provider-no-email | first filled button | 0/-72 | 0/-72 |  |
| Signup | Signup-finish | mark | 0/0 | 0/0 | different page: no email field, chip added |
| Signup | Signup-finish | h1 | 0/0 | 0/0 |  |
| Signup | Signup-finish | first filled button | 0/+4 | 0/-32 |  |
| Security | Security-linked | h1 | 0/0 | 0/0 | status line (+20) in Google/Microsoft section pushes later sections; not reserved |
| Security | Security-linked | h2 Recent security activity | n/a | 0/+20 |  |
| Security | Security-linked | h2 Where you’re signed in | n/a | 0/+20 |  |
| Security | Security-link-error | h1 | 0/0 | 0/0 | error slot (+48) in section pushes later sections; not reserved |
| Security | Security-link-error | h2 Recent security activity | 0/+68 | 0/+48 |  |
| Security | Security-link-error | h2 Where you’re signed in | 0/+68 | 0/+48 |  |
| Security | Security-passkey-added | h1 | 0/0 | 0/0 | new passkey row (+96..144) pushes later sections (expected) |
| Security | Security-passkey-added | h2 Google and Microsoft | 0/+144 | 0/+96 |  |
| Security | Security-passkey-added | h2 Recent security activity | 0/+144 | 0/+96 |  |
| Security | Security-passkey-added | h2 Where you’re signed in | 0/+144 | 0/+96 |  |
| Security | Security-passkey-limit | h1 | 0/0 | 0/0 | 10 rows (+724..1176) pushes later sections (expected) |
| Security | Security-passkey-limit | h2 Google and Microsoft | 0/+1176 | 0/+724 |  |
| Security | Security-passkey-limit | h2 Recent security activity | 0/+1176 | 0/+724 |  |
| Security | Security-passkey-limit | h2 Where you’re signed in | 0/+1176 | 0/+724 |  |
| Security | Security-load-error | h1 | 0/0 | 0/0 | alert replaces list: later sections move up |
| Security | Security-load-error | h2 Google and Microsoft | 0/-192 | 0/-100 |  |
| Security | Security-load-error | h2 Recent security activity | 0/-192 | 0/-100 |  |
| Security | Security-load-error | h2 Where you’re signed in | 0/-192 | 0/-100 |  |
| Security | Security-recovered | h1 | 0/0 | 0/0 | alert above first section (+148/+244, 4.10 exception); content also differs (see RIG-06) |
| Security | Security-recovered | h2 Email codes | 0/+244 | 0/+148 |  |
| Security | Security-recovered | h2 Google and Microsoft | 0/+100 | 0/+52 |  |
| Security | Security-recovered | h2 Passkeys | 0/+244 | 0/+148 |  |
| Security | Security-recovered | h2 Recent security activity | 0/-232 | 0/-164 |  |
| Security | Security-recovered | h2 Where you’re signed in | 0/+80 | 0/+32 |  |

## 4. Screenshot review

Looked at myself (contact sheets built from the rig's shots, in `/private/tmp/claude-501/-Users-akakijomidava-orca-workspaces-Richie-cetacean/300c9cb1-b437-49c8-ae7f-96f52969e541/scratchpad/sheets/`):
- All 31 base boards (B) at 320 and 1440, light: `b320-1..6.png`, `b1440-1..6.png`. No overflow, clipping or overlap at 320; mascot, h1 and footer sit on the documented positions.
- All dark wrappers (Main, Signedout, Security, Timeout, Error, Signin-offline, Signin-provider-error, Recover) at 1440 and six at 390: `dk-1.png`, `dk390-1.png`. Badges invert correctly; brand marks readable on black; no light-drawn pieces.
- 200% zoom (640x400) for 10 boards (Main, Security, Recover-details, Signup, Recover-approved, Provider-link-code, Error, Signin-provider-error, Recover, Signedout-prompt): `z200-1.png`, `z200-2.png`. No horizontal scroll, no clipping.
- Forced colours, 14 boards spanning edge, settings, recovery, SSO and email (Main, Security, Recover-details, Error-link, Signin-provider-error, Provider-link, Recover-approved, Signup, Email-recovery-alert, Security-unlink, Session-expired, Signedout-prompt, plus forced-dark Security-linked, Recover-code, Error-link): `fc-1.png`, `fc-2.png`, `fcd-1.png`. Borders, badges, mascot marks, the drawn OTP focus ring and dialogs all stay visible; only RIG-08.

Not covered by the rig and not tested: real screen reader output, hover states, the canvas-only `.is-focused` ring against a real focus ring.
