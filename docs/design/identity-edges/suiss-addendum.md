# suiss addendum for identity edge pages (WP1.1)

Scope: edge pages (expired request, generic error, offline, rate limit, signed out), account recovery, and SSO placement. Extends `../suiss-ui-guide.md` (§4.7, §7) and `../suiss-blocks-catalog.md` (§2.7, §2.9); does not repeat them.

**Sources read from registry JSON (`https://suiss.app/ui/r/base-suiss/{name}.json`, `files[0].content`), all fetched OK:** blocks `empty-offline`, `empty-search`, `login-account`, `login-passkey`, `login-sheet`, `signup-form`, `signup-steps`, `verify-device`, `verify-email`, `verify-code`; ui `alert`, `empty`, `spinner`, `button`, `separator`, `field` (for `FieldSeparator`), `input-otp`, `questionnaire`, `otp-autofill`; examples `field-separator-with-text`, `spinner-in-button`, `input-otp-alphanumeric`, `input-otp-invalid`, `toast-with-action`, `progress-indeterminate`. Nothing failed to fetch. Registry scan (names and descriptions) found **no** block or component for: social/provider/OAuth/SSO buttons, 404/500/error pages, status pages, rate limit, or standalone countdown. The closest are listed below. "(inferred)" marks anything not read from source.

## 0. Findings in one table

| Need | suiss has | Verdict |
|---|---|---|
| Offline / retry | `empty-offline` | Good skeleton, marketing copy, no failure path |
| Error / 404 / 500 / expired / signed out | nothing; `empty` + the auth frame | Compose |
| Provider buttons, SSO | nothing; `login-sheet` stack + `field-separator-with-text` divider | Compose |
| Multi-step | `signup-steps` (dots), `questionnaire` (progress bar), `bag-checkout` (not read, commerce) | Borrow dots + Back/Continue |
| Device wait | `verify-device` left column | Borrow; add timeout and cancel |
| Countdown | `verify-email` button, `verify-code` text | Borrow the two shapes |
| Rate limit | nothing | Compose `Alert` + countdown |

## 1. Offline and retry (`empty-offline`)

Markup (offline branch):
`section.flex.min-h-120.items-center.justify-center.p-6` > `Empty.gap-5.p-0` > `EmptyHeader.max-w-95.gap-3.5` (`EmptyMedia.mb-0.text-muted-foreground` with a bare `size-13` icon, `EmptyTitle.text-3xl.tracking-tight`, `EmptyDescription.text-lg/normal`) then `EmptyContent.flex-row.justify-center.gap-3.5` with two `Button size="lg"`.

Retry button: `<Button size="lg" onClick=... disabled={state==="loading"} aria-live="polite">{loading && <Spinner/>}{loading ? "Connecting" : "Try Again"}</Button>`. Copy shape: title is a full sentence with a period ("You're not connected to the internet."), description is an instruction plus a reassurance. Busy state is a fixed 1.4 s timer that always succeeds.

Borrow:
- Layout and classes above, with copy such as "You're offline." / "Check your connection, then try again. We'll keep what you typed."
- Busy shape: disable, `Spinner` before the label, label changes to a gerund. `spinner-in-button` shows the preferred slot form: `<Spinner data-icon="inline-start" />`. Use that, not the bare `<Spinner />` that `empty-offline` uses (the `data-icon` padding rules in `button` only apply with the attribute).
- Detect online/offline from `navigator.onLine` plus the `online` event, and auto-retry when it fires (inferred).

Avoid:
- `aria-live="polite"` on the button itself while it is `disabled`: a disabled button is dropped from focus and many screen readers will not announce its text change. Put the live region on a sibling `p` (as `verify-device` does with `div.min-h-16[aria-live]`).
- Label swap with no width lock ("Try Again" to "Connecting" changes width; the button jumps). Add `min-w-32` (inferred).
- `Spinner` ships `role="status" aria-label="Loading"` and `animate-[spin_0.8s_steps(8)_infinite]` with **no** `motion-reduce:` variant. Inside a labelled button the "Loading" status duplicates the visible text; set `aria-hidden` on it there (inferred), and accept the steps animation under reduced motion or add `motion-reduce:animate-none` (inferred).
- Second button "Downloads", the music-album grid, the Unsplash images, "Disconnect ›". Retry must also have a failure outcome ("Still offline", re-enable the button); the block has none (inferred).

## 2. Full-page error and empty layouts

Two layout families exist in suiss; pick one for all edge pages.

| | Centred `Empty` (`empty-offline`) | Auth frame (`login-passkey`, `verify-*`) | Start-aligned (`empty-search`) |
|---|---|---|---|
| Wrapper | `section.flex.min-h-120.items-center.justify-center.p-6` | `section.flex.min-h-150.items-center.justify-center.p-6` | inside a page column |
| Icon | bare `size-13`, `text-muted-foreground` | disc `flex size-16`/`size-18 items-center justify-center rounded-full bg-muted text-primary`, icon `size-7.5`/`size-8.5` | none |
| Title | `EmptyTitle text-3xl tracking-tight` (a `div`) | `h1 text-4xl font-semibold tracking-tight text-balance md:text-5xl`, `mt-5` | `EmptyTitle text-3xl tracking-tight` |
| Lead | `text-lg/normal` | `mt-3 max-w-110 text-lg text-muted-foreground` | `text-lg/normal` |
| Actions | `flex-row justify-center gap-3.5`, `lg` | `mt-7.5 flex flex-wrap justify-center gap-3.5`, `lg` | secondary chips `h-8.5 px-3.5` |
| Alignment | centred | centred | `Empty className="items-start gap-3 rounded-none p-0 text-start"`, `EmptyHeader className="max-w-none items-start"` |

Recommendation: edge pages are sign-in siblings, so use the **auth frame** (disc + `h1 text-4xl md:text-5xl`) so Richie reads as one product; keep `Empty` for inline "nothing here" inside settings (already catalog §2.9). Centred, single column, at most two buttons, one primary. Start-aligned is only apt for in-page results.

Details to carry over:
- `EmptyTitle` is a `div`, not a heading. A full-page error needs a real `h1`; `EmptyDescription` is also a `div`. (read from `empty`)
- `Empty` has `border-dashed` but no border width, so it renders borderless unless `border` is added; `p-12 rounded-2xl` defaults are overridden by `p-0` in both blocks.
- Success/neutral tail state in `verify-email`, `verify-code`, `signup-steps`: `span.size-19.rounded-full` green (`bg-[color-mix(in_oklab,var(--color-green-500),var(--foreground)_25%)] text-white`, dark: `...black_35%`), `h1.text-4xl`, `p.text-lg.text-muted-foreground`, enter motion `starting:translate-y-4.5 starting:opacity-0 motion-reduce:transition-none`. Use this for "You're signed out" only if the tone is positive; a neutral `bg-muted text-primary` disc is safer (inferred).
- Error tone: suiss has no red full-page treatment. `Alert variant="destructive"` only tints the icon (`*:[svg]:text-destructive`); the card stays `bg-muted`. Keep that restraint: red icon, never a red page.
- Min heights are fixed `min-h-120` to `min-h-215`; for real routes use `min-h-svh` (inferred).
- Avoid: "suiss Account", "Jamie", "Start over ›" demo links, `Demo code: 482913` hint line in `verify-code`.

## 3. Provider buttons, "or" divider, SSO

suiss has **no** provider-button block. Pieces that exist:

- `login-sheet` stack: `div.flex.w-full.flex-col.gap-2.5` containing `Button size="xl" block className="h-12.5 bg-foreground text-lg font-semibold text-background hover:bg-foreground/90"` ("Sign in with suiss") then `Button size="xl" block variant="secondary" className="h-12.5 text-lg font-semibold"` ("Continue with email"). Verb is "Sign in with" / "Continue with".
- `field-separator-with-text`: `FieldGroup.w-96` > `Field` > `Button variant="outline"` "Continue with Apple", then `<FieldSeparator>Or continue with</FieldSeparator>`, then the email `Field`. `FieldSeparator` = `div.-my-2.h-5.text-sm.relative` with `Separator.absolute.inset-0.top-1/2` and label `span.px-2.text-muted-foreground.relative.mx-auto.block.w-fit.bg-background`. The label masks the line with `bg-background`, so it fails on a `bg-muted` card (inferred); use it on the page background only.
- `button` slots: `data-icon="inline-start"` gives `has-data-[icon=inline-start]:ps-3` (lg: `ps-4.5`); icon default `size-4`. `block` adds `w-full` **and** swaps the pill for `blockRadius`: `lg` `rounded-lg`, `xl` `rounded-xl`, default `rounded-md`. So a `block` button is not a pill; a plain `className="w-full"` stays `rounded-full`. Mixing them in one stack looks inconsistent.
- `separator`: `shrink-0 bg-border data-horizontal:h-px data-horizontal:w-full data-vertical:w-px data-vertical:self-stretch`.

Recommended composition (inferred, from the above):
1. Order on `/sign-in`: email field + `Button size="lg"` Continue (primary), `FieldSeparator` "or", then provider buttons, then "Use a passkey" as `login-passkey`'s text link (`mt-4.5 text-sm text-primary`).
2. Provider button: `Button variant="outline" size="lg" block` with `<svg data-icon="inline-start" className="size-4.5">` brand mark + "Continue with Google". Same variant and size for Microsoft and any other; no per-brand colours on the button. Use the vendors' official multi-colour marks as SVG (not `currentColor`), and follow each vendor's brand rules for label wording. Stack `flex flex-col gap-2.5` (the `login-sheet` gap), full width of the `w-115 max-w-full` form column, never side by side.
3. Work SSO is not a button: after the email is entered, route by domain and swap the primary label to "Continue with SSO" / redirect (like `login-account`'s email-then-reveal, which expands via `grid-rows-[0fr]` to `[1fr]`, `duration-450`). Give a fallback link "Sign in with a code instead ›". In settings use an `Item` row (catalog §2.3) with a `Badge`, not a button stack.
4. Do not use the inverted primary (`bg-foreground text-background`): it is a brand-specific Apple-style look and competes with Continue.
5. Disabled/busy provider button: `disabled` + `Spinner data-icon="inline-start"` and label "Redirecting to Google" (inferred).

## 4. Multi-step flows (`signup-steps`, `questionnaire`)

`signup-steps` markup:
- Dots: `ol[aria-label="Progress"].flex.gap-2`, each `li` `h-1.5 w-1.5 rounded-full bg-secondary transition-[width,background-color] duration-400 data-reached:bg-primary aria-[current=step]:w-7 motion-reduce:transition-none`, `aria-current="step"`, hidden `span.sr-only` "Step 2, complete".
- Visible text above the title: `p.text-sm.font-semibold.text-muted-foreground` "Step 1 of 3". Title `h1.text-4xl.md:text-5xl`, lead `text-lg text-muted-foreground`.
- Field: grouped border `div.mt-4.overflow-hidden.rounded-xl.border.border-input` + `Input.h-13.rounded-none.px-4.text-lg`, `autoFocus` only when `index > 0`.
- Footer: `div.flex.justify-center.gap-4` with `Button size="lg" variant="outline"` Back (rendered only when `index > 0`), and `Button size="lg" disabled={!ready} className="min-w-35"` Continue; the last step relabels it ("Create account").
- Back behaviour: pure state, values retained, slide direction `--from: ±40px` with `starting:translate-x-(--from) rtl:starting:-translate-x-(--from)`. Enter submits the form (`noValidate`).

`questionnaire` (ui): `QuestionnaireProgress` is `role="progressbar" aria-live="polite" aria-valuetext="Question 2 of 5"`, `text-xs tabular-nums min-w-[14ch]`; parts `QuestionnairePrevious` / `Skip` / `Next` / `Submit`, Enter shortcut via `aria-keyshortcuts`.

Borrow for recovery (inferred sequence: confirm account email, choose how to prove identity, submit request, "request received"): keep dots + "Step n of m" text, Back as `outline`, one primary. Keep it to 3 steps; a recovery request is short, so the dots are optional and "Step n of m" alone is enough. Retain entered values on Back. Put a Cancel/"Back to sign in ›" text link (`mt-4.5 text-sm text-primary`) on step 1, because Back is hidden there.

Avoid: Continue `disabled` until valid with no explanation (an unexplained disabled primary; show the error line `mt-2.5 min-h-5.5 text-sm text-destructive aria-live="polite"` from `login-account` instead); footer buttons outside the `<form>` using `onClick` (no real submit; keep the button `type="submit"` with `form=`); no focus move to the new step heading (set focus to `h1` with `tabIndex={-1}` on step change, inferred); dots with only `aria-current` and no live announcement.

## 5. Device verification / waiting (`verify-device`)

- Layout: `section.grid.min-h-150.bg-background.md:grid-cols-2`. Left: `div.flex.flex-col.justify-center.gap-3.5.px-6.py-12.md:px-12`, start-aligned: disc `size-14` (`bg-muted text-primary`, icon `size-6.5`), `h1.text-4xl.text-balance`, `p.max-w-95.text-lg.text-muted-foreground`. Right: a simulated phone alert on a blurred Unsplash backdrop (`role="alertdialog"`, `bg-popover/86 backdrop-blur-2xl`). **Do not borrow the right half**; it is a prop, and `role="alertdialog"` on a decoration is wrong.
- Waiting line: `div.mt-2.5.min-h-16[aria-live="polite"]` containing `p.flex.items-center.gap-2.5.text-sm.text-muted-foreground` > `<Spinner/>` "Waiting for approval…". Phases: ask, code (`InputOTP` with "Incorrect code."), deny ("Sign-in denied." + "Try again ›"), ok.
- Borrow: the left-column frame for "Check your device" style steps (e.g. approve from a signed-in session during recovery), the `min-h-16` reserved status area (no layout shift), "Try again ›" text link for the denied state.
- Avoid / add (inferred): the wait never times out and has no Cancel. Add a visible expiry ("This request expires in 10 minutes"), a "Use a different method ›" link, and an expired state. Deny copy "change your password" does not apply to a passwordless product.
- `verify-email` (the email-link wait): `span.relative.h-18.w-24` gradient mail tile (`bg-linear-160 from-cyan-500 to-primary`) with a red "1" badge (`sr-only` "new message"), `h1.mt-6.5`, address in `strong.font-semibold.text-foreground`, actions `mt-7.5 flex flex-wrap justify-center gap-3.5`, footnote `mt-6.5 text-sm text-muted-foreground` "Didn't get it? Check your junk folder or change the address ›". Borrow the footnote shape and bold-the-address pattern; drop the gradient tile and fake notification badge (marketing flavour). "Open Mail" cannot be implemented reliably (no universal URL); omit.

## 6. Countdown, cooldown, rate limit

Two shapes exist, both timer-driven by `setTimeout(...,1000)` in an effect:
- `verify-email`: `<Button size="lg" variant="outline" disabled={left>0} className="tabular-nums">{left>0 ? `Sent again · ${left}` : "Resend"}</Button>`, 45 s. Problems: bare number with no unit; `disabled` drops it to `opacity-50`, so the label may fail contrast; not announced.
- `verify-code`: `div.mt-6.flex.flex-col.items-center.gap-2.5.text-sm`; while running `span.text-muted-foreground.tabular-nums` "Resend code · 0:30" (30 s); afterwards `button.rounded-xs.text-primary.hover:underline` "Resend code ›". Not live. Better of the two: plain text, not a disabled control.

No rate-limit presentation exists. Recommendation (inferred): use `Alert` (default variant, `bg-muted`, icon `text-primary`; `destructive` only if the user is hard-blocked) with `AlertTitle` "Too many attempts" and `AlertDescription` "Try again in 12 minutes." plus `AlertAction` (absolute `top-4 inset-e-4`, adds `pe-24` to the card; hide it on narrow screens or it crowds the text). Read the server's `Retry-After`; show `m:ss` with `tabular-nums` under 1 minute and whole minutes above, rounding up; do not tick the live region each second, announce once at start and once when it ends ("You can try again."). Re-enable the primary button at expiry. For 429 on the code entry, keep the slots visible but `disabled` (`InputOTP` sets `opacity-50` via `in-[[data-input-otp-container]:has(input:disabled)]`).

`Alert` caveat: it hard-codes `role="alert"` (assertive). Fine for an arriving rate-limit message; for static explanatory text on a page, use a `p`. It has no success or warning variant, only `default` and `destructive`.

## 7. Other things the designer should know

- **OTP is the biggest mismatch.** Slots are `h-14 w-11 sm:h-15.5 sm:w-12.5`, groups `gap-2.5`, between groups `gap-4`. Eight slots need 8x44 + 6x10 + 16 = 428 px (476 px at `sm`) against 312 px of usable width at a 360 px phone with `p-6` (calculated). Re-derive: 4+4 with `w-8`/`gap-1.5` and a `InputOTPSeparator`, or two rows. Alphanumeric works: `pattern={REGEXP_ONLY_DIGITS_AND_CHARS}` (`input-otp-alphanumeric`); the default pattern is digits only (inferred from the library). Show characters uppercase and accept lowercase paste (inferred). No block sets `autoComplete="one-time-code"` (searched); add it.
- **Error feedback on code entry** (`verify-code`): on complete, 200 ms delay, `groupRef.current.animate([0,-10,9,-6,4,0] translateX, {duration:420})`, skipped when `matchMedia("(prefers-reduced-motion: reduce)")`; slots turn `group-data-[status=error]/code:border-destructive`; status line `mt-3.5 min-h-5 text-sm aria-live="polite"` "Incorrect code."; value cleared after 800 ms. Keep, but say how many attempts remain, and add expiry ("code expired" state is not in suiss).
- `otp-autofill` (ui): states `idle | filling | verifying | verified | invalid`; WebOTP via `navigator.credentials.get({otp:{transport:["sms"]}})`. SMS-only, so not useful for email codes; skip.
- **Passkey failure is missing.** `login-passkey` has only scan (Dialog, `w-65 rounded-3xl bg-popover/82 backdrop-blur-2xl`) and ok; Cancel is `Button variant="plain"`. Design explicitly: cancelled, no passkey on this device, unsupported browser, with "Email me a code ›" as the way out (inferred).
- **Password leftovers to strip:** `login-account` "Forgot password? ›", "Remember me" `Checkbox`, placeholder "Email or phone number", and `login-passkey` "Sign in with password ›". Its email-then-reveal pattern and `SubmitArrow` (`Button variant="outline" size="icon-sm"`, spinner swaps the arrow while `busy`, `rtl:rotate-180`) still fit "email then code".
- **Field error style:** grouped border container turns `data-invalid:border-destructive data-invalid:bg-destructive/5 dark:data-invalid:bg-destructive/10`; shake on error; message line reserves `min-h-5.5`.
- `toast-with-action`: `toast.add({title, description, actionProps:{children:"Undo", onClick}})`. Use toasts for transient success ("Link sent"), never for errors the user must act on.
- `progress-indeterminate`: `<Progress value={null}>` with `ProgressLabel`; an option for "Signing you in…" instead of a spinner (inferred).
- Landmarks: the blocks are bare `section`s with an `h1`; the real pages need `main` and a skip target (inferred). Links in blocks use the trailing `›`; keep it as the one text-link convention.
